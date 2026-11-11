-- Ett ledigt rum visas och ett upptaget rum filtreras bort.
SELECT * FROM rooms
         WHERE NOT EXISTS(SELECT 1 FROM bookings
         WHERE room_id = rooms.id
         AND start_date < '2026-12-13 15:00:00' -- Utchekning
         AND end_date > '2026-12-11 11:00:00'   -- Inchekning
);

-- Ett rum kan bokas samma dag som en tidigare kund checkar ut.
SELECT * FROM rooms
         WHERE NOT EXISTS(SELECT 1 FROM bookings
         WHERE room_id = rooms.id
         AND start_date < '2026-12-15 11:00:00' -- Utchekning
         AND end_date > '2026-12-13 15:00:00'   -- Inchekning
);


-- Överlappande bokningar nekas, även när den ena perioden helt omsluter den andra.
INSERT INTO bookings
    (customer_id, room_id, start_date, end_date, num_persons)
VALUES (3, 14, '2026-12-18', '2026-12-22', 2 );


-- Ett för stort sällskap inte kan boka rummet
INSERT INTO bookings
    (customer_id, room_id, start_date, end_date, num_persons)
VALUES (3, 1, '2027-01-01', '2027-01-04', 2 );


-- Kombinerade sökfilter och båda sorteringsalternativen fungerar.
SELECT * FROM hotels WHERE city = 'Visby'
    AND has_restaurant = 1
    AND distance_to_beach <= 200
    ORDER BY rating DESC;

SELECT * FROM hotels WHERE city = 'Visby'
    AND has_restaurant = 1
    AND distance_to_beach <= 200
    ORDER BY rating ASC;


-- Ändring av antal personer följer rummets kapacitet.
UPDATE bookings
SET num_persons = 3
WHERE id = 20;


-- Avbokning frigör rummet.
DELETE FROM bookings WHERE id = 17;






-- Lägger i en ny kund i databasen
INSERT INTO customers (name, last_name, email, telephone)
VALUES ('Johan', 'Karlsson', 'johan.k@example.com', '0731234567');


-- Väljer en befintlig kund inför en bokning
SELECT id, name, last_name, email, telephone
FROM customers
WHERE last_name LIKE 'Karlsson%';


-- Söker efter lediga rum.
SELECT
    h.name AS hotel_name,       -- Visa boendets namn
    h.city AS ort,               -- Visa ort
    r.name AS room_name,         -- Visa rum
    r.capacity,                  -- Visa kapacitet
    r.price                      -- Visa pris
FROM rooms r
JOIN hotels h ON r.hotel_id = h.id
WHERE
  -- 1. Grundkrav: Ange ort och antal personer (tillräcklig kapacitet)
  h.city = 'Visby'
  AND r.capacity >= 2

  -- 2. Grundkrav: Lediga under hela perioden (inchecknings- och utcheckningsdatum)
  AND NOT EXISTS (
      SELECT 1
      FROM bookings b
      WHERE b.room_id = r.id
        AND '2026-12-18 15:00:00' < b.end_date
        AND '2026-12-20 11:00:00' > b.start_date
  )

  -- 3. Valfria filter: Pool och Restaurang måste finnas (1 = Ja, krävs)
  AND h.has_pool = 1
  AND h.has_restaurant = 1

  -- 4. Valfria filter: Maximalt avstånd till strand och centrum i meter
  AND h.distance_to_beach <= 500
  AND h.distance_to_city_center <= 500

-- 5. Sorteringsalternativ (Kör en i taget genom att ta bort '--')
ORDER BY r.price ASC;          -- Alternativ A: Pris per natt, lägst först
-- ORDER BY h.rating DESC;     -- Alternativ B: Boendets omdöme, högst först


-- Bokar ett valt rum åt kunden
INSERT INTO bookings (customer_id, room_id, start_date, end_date, num_persons)
VALUES (1, 5, '2027-01-15 15:00:00', '2027-01-18 11:00:00', 2);


-- Visar en bdkräftelse med bokningsnummer och totalpris antal nätter x pris per natt
SELECT
    b.id AS booking_number,
    b.start_date AS incheckning,
    b.end_date AS utcheckning,
    b.num_persons AS antal_personer,
    DATEDIFF(b.end_date, b.start_date) AS antal_nätter,
    r.price AS pris_per_natt,
    -- Hela den matematiska uträkningen sker här på databassidan:
    (DATEDIFF(b.end_date, b.start_date) * r.price) AS total_price
FROM bookings b
JOIN rooms r ON b.room_id = r.id
WHERE b.id = LAST_INSERT_ID();

-- Visa bokningsnummer, boende, rum, datum, antal personer och totalpris.
SELECT
    b.id AS booking_number,
    h.name AS boende,
    r.name AS rum,
    b.start_date AS incheckning,
    b.end_date AS utcheckning,
    b.num_persons AS antal_personer,
    -- Databasen räknar ut totalpriset direkt: nätter * rumspris
    (DATEDIFF(b.end_date, b.start_date) * r.price) AS total_price
FROM bookings b
JOIN rooms r ON b.room_id = r.id
JOIN hotels h ON r.hotel_id = h.id
WHERE b.customer_id = 1
ORDER BY b.start_date ASC;

-- Ändra antalet personer i en bokning
UPDATE bookings
SET num_persons = 3
WHERE id = 26;

-- AVBOKA
DELETE FROM bookings
WHERE id = 27;
