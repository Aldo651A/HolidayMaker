### Holday Marker -Bokningssystem

## Kom igång

- Ha en MySQL-server tillgänglig(version 8.0.16 eller senare) 
- Steg 1. Skpa databasen. Kör SQl-scriptet ("datebase/schema.sql") i din databasklient.
Scriptet skapar databasen holiday_maker och alla fyra tabeller ('customer, hotels, rooms, bookongs)
samt triggers som automatisk stoppar dubbelbokningar och kontrollerar rummens kapacitet.

- Steg 2. Lägg in startdata och nkör därefter ("datebase/testdata.sql") för att fylla tabellerna 
med customers, hotels, rooms och bookings. Obs! kör inte detta script upprepade gånger eftersom 
INSERT då kan lägga in dubbletter.

- Steg 3. Det finns färdiga testfrågor i "datebase/queries.sql" om man nu vill köra dessa en i taget.
För att kontrollera om sökfilter och triggers fungerar. Detta kan göras i själva databasen.

-Steg 4. Konfigurera C#-anslutningen. Öppna ("Proram.cs") i projektets huvudmapp och ändra 
lösenordet i "connectionString" så att det stämmer med ditt lösenord och MySQ-konto.

- Denna raden finns i Program.cs.
string connectionString = "Server=localhost;Database=holiday_maker;User ID=root;Password=DITT_LÖSENORD;UseAffectedRows=false;";

- Steg 5. Starta programmet. Öppna en terminal i projektmappem och kör.
Se till att du är i Git Bash och kör fäljande kommandon. "dotnet run"

## Programflödet i menyn

- Programmet styrs av en meny i konsolen och det är operatören som väljer önskad resultat.

- "Registrerar helt ny kund": Sparar kunden uppgifter i detta fall ID nummer och aktiverar kunden.

- "Sök och välj en befintlig kund": man kan ange efternamn eller visa alla kunder från listan kunder
och aktivedrar den kund man har valt.

-"Sök lediga rum": härmtar rum från listan ttillgängliga rum baserat på ort, in och utchekningsdatum, 
antal personer samt valfria filters som pool, restaurang, avstånd till strnden och centrum, pris eller omdöme.

- "Skapa bokning": Visar tillgängliga rum skapar en bokning i bookings för vald kund och räknar ut total
priset alltså (antal nätter x rumpris)

- Visa kundens bokningar: Denna visar en kunds alla bokningar som en lista.

- Avboka: Visar kundens bokningar och tar bort vald bokning så att rummet blir tllgänglig igen.

- Ändra antalet personer i en bokning: Uppdaterar antalet personer, databasen kontrollerar med
trigger om det nya antalet personer rymms i rummet.

## Kodsruktur

- Program.cs öppnar databasanslutningen, skapar en Bookrepository och kör huvudmenyn.
Anslutningen ägs och stängs automatiskt av Program.cs via await using.
BookingRepository.cs samlar alla databasmetoder och parametriserade SQL-frågor  
(@-parametrar och PrepareAsync()) för att skydda mot SQL-injections.
Customer.cs, AvailableRoom.cs och BookingDetails nnehåller de record-modeller 
som används för att ta emot och skriva ut data från databasen.
