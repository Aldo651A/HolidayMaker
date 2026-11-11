create table customers
(
    id        int auto_increment
        primary key,
    name      varchar(70)  not null,
    last_name varchar(70)  not null,
    email     varchar(100) not null,
    telephone varchar(30)  not null
);

create table hotels
(
    id                      int auto_increment
        primary key,
    name                    varchar(60) not null,
    city                    varchar(60) not null,
    has_pool                tinyint(1)  not null,
    has_restaurant          tinyint(1)  not null,
    distance_to_beach       int         not null,
    distance_to_city_center int         not null,
    rating                  int         not null
);

create table rooms
(
    id       int auto_increment
        primary key,
    name     varchar(50)   not null,
    capacity int           not null,
    price    decimal(9, 2) not null,
    hotel_id int           not null,
    constraint rooms_rooms_id_fk
        foreign key (hotel_id) references hotels (id)
);

create table bookings
(
    id          int auto_increment
        primary key,
    customer_id int      not null,
    room_id     int      not null,
    start_date  datetime not null,
    end_date    datetime not null,
    num_persons int      not null,
    constraint bookings_customers_id_fk
        foreign key (customer_id) references customers (id),
    constraint bookings_rooms_id_fk
        foreign key (room_id) references rooms (id)
);

create definer = root@`%` trigger check_and_adjust_booking
    before insert
    on bookings
    for each row
BEGIN
    -- Variables to hold the number of double-bookings and the room capacity
    DECLARE conflicts INT;
    DECLARE room_capacity INT;

    -- Step A: Force the times to be check-in at 15:00 and check-out at 11:00
    SET NEW.start_date = CAST(CONCAT(DATE(NEW.start_date), ' 15:00:00') AS DATETIME);
    SET NEW.end_date = CAST(CONCAT(DATE(NEW.end_date), ' 11:00:00') AS DATETIME);

    -- Step B: Find the maximum capacity of the chosen room
    SELECT capacity INTO room_capacity
    FROM rooms
    WHERE id = NEW.room_id;

    -- Step C: Check if the number of guests is larger than the room capacity
    IF NEW.num_persons > room_capacity THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Det går inte att boka: Sällskapet är för stort för detta rum!';
    END IF;

    -- Step D: Count how many bookings already exist for this room during these dates
    SELECT COUNT(*) INTO conflicts
    FROM bookings
    WHERE room_id = NEW.room_id
      AND NEW.start_date < end_date
      AND NEW.end_date > start_date;

    -- Step E: If there is a double-booking, stop and show an error
    IF conflicts > 0 THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Det går inte att boka: Rummet är redan upptaget under denna period!';
    END IF;
END;

create definer = root@`%` trigger check_and_adjust_booking_update
    before update
    on bookings
    for each row
BEGIN
    -- Variables to hold the number of double-bookings and the room capacity
    DECLARE conflicts INT;
    DECLARE room_capacity INT;

    -- Step A: Force the times to be check-in at 15:00 and check-out at 11:00
    SET NEW.start_date = CAST(CONCAT(DATE(NEW.start_date), ' 15:00:00') AS DATETIME);
    SET NEW.end_date = CAST(CONCAT(DATE(NEW.end_date), ' 11:00:00') AS DATETIME);

    -- Step B: Find the maximum capacity of the chosen room
    SELECT capacity INTO room_capacity
    FROM rooms
    WHERE id = NEW.room_id;

    -- Step C: Check if the updated number of guests is larger than the room capacity
    IF NEW.num_persons > room_capacity THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Det går inte att ändra bokningen: Sällskapet är för stort för detta rum!';
    END IF;

    -- Step D: Count how many OTHER bookings already exist for this room during these dates
    -- We exclude the current booking's ID (NEW.id) so it doesn't collide with itself during an update
    SELECT COUNT(*) INTO conflicts
    FROM bookings
    WHERE room_id = NEW.room_id
      AND id != NEW.id
      AND NEW.start_date < end_date
      AND NEW.end_date > start_date;

    -- Step E: If there is a double-booking, stop and show an error
    IF conflicts > 0 THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Det går inte att ändra bokningen: Rummet är redan upptaget under denna period!';
    END IF;
END;

