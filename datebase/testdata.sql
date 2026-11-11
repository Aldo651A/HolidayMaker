-- Detta är data för bokings
INSERT INTO bookings (id, customer_id, room_id, start_date, end_date, num_persons) VALUES (1, 1, 17, '2026-12-18 15:00:00', '2026-12-20 11:00:00', 1);
INSERT INTO bookings (id, customer_id, room_id, start_date, end_date, num_persons) VALUES (2, 3, 4, '2026-12-14 15:00:00', '2026-12-17 11:00:00', 4);
INSERT INTO bookings (id, customer_id, room_id, start_date, end_date, num_persons) VALUES (3, 2, 4, '2026-12-19 15:00:00', '2026-12-25 11:00:00', 2);
INSERT INTO bookings (id, customer_id, room_id, start_date, end_date, num_persons) VALUES (4, 5, 14, '2026-12-04 15:00:00', '2026-12-07 11:00:00', 4);
INSERT INTO bookings (id, customer_id, room_id, start_date, end_date, num_persons) VALUES (5, 5, 6, '2026-12-20 15:00:00', '2026-12-22 11:00:00', 1);
INSERT INTO bookings (id, customer_id, room_id, start_date, end_date, num_persons) VALUES (6, 3, 8, '2026-12-23 15:00:00', '2026-12-26 11:00:00', 1);
INSERT INTO bookings (id, customer_id, room_id, start_date, end_date, num_persons) VALUES (7, 4, 1, '2026-12-01 15:00:00', '2026-12-03 11:00:00', 1);
INSERT INTO bookings (id, customer_id, room_id, start_date, end_date, num_persons) VALUES (8, 4, 5, '2026-12-27 15:00:00', '2026-12-30 11:00:00', 3);
INSERT INTO bookings (id, customer_id, room_id, start_date, end_date, num_persons) VALUES (9, 4, 16, '2026-12-14 15:00:00', '2026-12-16 11:00:00', 2);
INSERT INTO bookings (id, customer_id, room_id, start_date, end_date, num_persons) VALUES (10, 3, 8, '2026-12-01 15:00:00', '2026-12-02 11:00:00', 1);
INSERT INTO bookings (id, customer_id, room_id, start_date, end_date, num_persons) VALUES (11, 2, 14, '2026-12-16 15:00:00', '2026-12-23 11:00:00', 4);
INSERT INTO bookings (id, customer_id, room_id, start_date, end_date, num_persons) VALUES (12, 5, 11, '2026-12-06 15:00:00', '2026-12-07 11:00:00', 2);
INSERT INTO bookings (id, customer_id, room_id, start_date, end_date, num_persons) VALUES (13, 3, 19, '2026-12-30 15:00:00', '2027-01-04 11:00:00', 1);
INSERT INTO bookings (id, customer_id, room_id, start_date, end_date, num_persons) VALUES (14, 3, 5, '2026-12-22 15:00:00', '2026-12-28 11:00:00', 4);
INSERT INTO bookings (id, customer_id, room_id, start_date, end_date, num_persons) VALUES (15, 4, 1, '2026-12-30 15:00:00', '2027-01-03 11:00:00', 1);
INSERT INTO bookings (id, customer_id, room_id, start_date, end_date, num_persons) VALUES (16, 4, 11, '2026-12-30 15:00:00', '2027-01-01 11:00:00', 2);
INSERT INTO bookings (id, customer_id, room_id, start_date, end_date, num_persons) VALUES (18, 1, 20, '2026-12-11 15:00:00', '2026-12-12 11:00:00', 1);
INSERT INTO bookings (id, customer_id, room_id, start_date, end_date, num_persons) VALUES (19, 1, 6, '2026-12-06 15:00:00', '2026-12-13 11:00:00', 1);
INSERT INTO bookings (id, customer_id, room_id, start_date, end_date, num_persons) VALUES (20, 4, 10, '2026-12-23 15:00:00', '2026-12-26 11:00:00', 2);
INSERT INTO bookings (id, customer_id, room_id, start_date, end_date, num_persons) VALUES (21, 5, 10, '2026-12-25 15:00:00', '2026-12-28 11:00:00', 1);
INSERT INTO bookings (id, customer_id, room_id, start_date, end_date, num_persons) VALUES (22, 2, 8, '2026-12-17 15:00:00', '2026-12-22 11:00:00', 1);
INSERT INTO bookings (id, customer_id, room_id, start_date, end_date, num_persons) VALUES (23, 5, 12, '2026-12-03 15:00:00', '2026-12-07 11:00:00', 2);
INSERT INTO bookings (id, customer_id, room_id, start_date, end_date, num_persons) VALUES (24, 1, 8, '2026-12-22 15:00:00', '2026-12-26 11:00:00', 1);
INSERT INTO bookings (id, customer_id, room_id, start_date, end_date, num_persons) VALUES (25, 4, 20, '2026-12-21 15:00:00', '2026-12-23 11:00:00', 2);
INSERT INTO bookings (id, customer_id, room_id, start_date, end_date, num_persons) VALUES (26, 1, 5, '2026-12-18 15:00:00', '2026-12-20 11:00:00', 3);


-- Deta är data för kunder
INSERT INTO customers (id, name, last_name, email, telephone) VALUES (1, 'Anders', 'Pettersson', 'anders.pettersson1@example.se', '070-450 80 76');
INSERT INTO customers (id, name, last_name, email, telephone) VALUES (2, 'Lars', 'Andersson', 'lars.andersson2@example.se', '070-306 69 83');
INSERT INTO customers (id, name, last_name, email, telephone) VALUES (3, 'Mikael', 'Eriksson', 'mikael.eriksson3@example.se', '070-312 46 69');
INSERT INTO customers (id, name, last_name, email, telephone) VALUES (4, 'Lars', 'Persson', 'lars.persson4@example.se', '070-738 53 60');
INSERT INTO customers (id, name, last_name, email, telephone) VALUES (5, 'Anna', 'Pettersson', 'anna.pettersson5@example.se', '070-395 65 69');
INSERT INTO customers (id, name, last_name, email, telephone) VALUES (8, 'Johan', 'Karlsson', 'johan.k@example.com', '0731234567');
INSERT INTO customers (id, name, last_name, email, telephone) VALUES (10, 'Arvid', 'Andersson', 'arvid45@hotmail.se', '0731234568');

-- Detta är data för olika hoteller
INSERT INTO hotels (id, name, city, has_pool, has_restaurant, distance_to_beach, distance_to_city_center, rating) VALUES (1, 'Strandhotell', 'Visby', 0, 1, 150, 500, 4);
INSERT INTO hotels (id, name, city, has_pool, has_restaurant, distance_to_beach, distance_to_city_center, rating) VALUES (2, 'Havsbris Resort', 'Visby', 0, 1, 50, 200, 5);
INSERT INTO hotels (id, name, city, has_pool, has_restaurant, distance_to_beach, distance_to_city_center, rating) VALUES (3, 'Hotell Havsutsikt', 'Båstad', 1, 1, 50, 100, 5);
INSERT INTO hotels (id, name, city, has_pool, has_restaurant, distance_to_beach, distance_to_city_center, rating) VALUES (4, 'Kustpärlan', 'Båstad', 0, 0, 300, 200, 4);
INSERT INTO hotels (id, name, city, has_pool, has_restaurant, distance_to_beach, distance_to_city_center, rating) VALUES (5, 'Sol & Bad Hotell', 'Marstrand', 1, 0, 150, 800, 3);


-- Detta är data för dem olika rummen
INSERT INTO rooms (id, name, capacity, price, hotel_id) VALUES (1, 'Enkelrum 100', 1, 850.00, 1);
INSERT INTO rooms (id, name, capacity, price, hotel_id) VALUES (2, 'Dubbelrum 101', 2, 1400.00, 1);
INSERT INTO rooms (id, name, capacity, price, hotel_id) VALUES (3, 'Familjerum 102', 4, 2500.00, 1);
INSERT INTO rooms (id, name, capacity, price, hotel_id) VALUES (4, 'Familjerum 103', 4, 2600.00, 1);
INSERT INTO rooms (id, name, capacity, price, hotel_id) VALUES (5, 'Familjerum 100', 4, 2600.00, 2);
INSERT INTO rooms (id, name, capacity, price, hotel_id) VALUES (6, 'Enkelrum 101', 1, 850.00, 2);
INSERT INTO rooms (id, name, capacity, price, hotel_id) VALUES (7, 'Enkelrum 102', 1, 850.00, 2);
INSERT INTO rooms (id, name, capacity, price, hotel_id) VALUES (8, 'Enkelrum 103', 1, 850.00, 2);
INSERT INTO rooms (id, name, capacity, price, hotel_id) VALUES (9, 'Enkelrum 100', 1, 850.00, 3);
INSERT INTO rooms (id, name, capacity, price, hotel_id) VALUES (10, 'Dubbelrum Standard 101', 2, 1400.00, 3);
INSERT INTO rooms (id, name, capacity, price, hotel_id) VALUES (11, 'Dubbelrum Standard 102', 2, 1400.00, 3);
INSERT INTO rooms (id, name, capacity, price, hotel_id) VALUES (12, 'Familjerum 103', 4, 2600.00, 3);
INSERT INTO rooms (id, name, capacity, price, hotel_id) VALUES (13, 'Dubbelrum Standard 100', 2, 1400.00, 4);
INSERT INTO rooms (id, name, capacity, price, hotel_id) VALUES (14, 'Familjerum 101', 4, 2600.00, 4);
INSERT INTO rooms (id, name, capacity, price, hotel_id) VALUES (15, 'Familjerum 102', 4, 2600.00, 4);
INSERT INTO rooms (id, name, capacity, price, hotel_id) VALUES (16, 'Dubbelrum Deluxe 103', 2, 1950.00, 4);
INSERT INTO rooms (id, name, capacity, price, hotel_id) VALUES (17, 'Dubbelrum Standard 100', 2, 1400.00, 5);
INSERT INTO rooms (id, name, capacity, price, hotel_id) VALUES (18, 'Dubbelrum Deluxe 101', 2, 1950.00, 5);
INSERT INTO rooms (id, name, capacity, price, hotel_id) VALUES (19, 'Familjerum 102', 4, 2600.00, 5);
INSERT INTO rooms (id, name, capacity, price, hotel_id) VALUES (20, 'Dubbelrum Standard 103', 2, 1400.00, 5);


