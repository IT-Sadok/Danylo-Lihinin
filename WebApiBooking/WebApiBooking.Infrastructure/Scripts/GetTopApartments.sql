SELECT
    a."Id"   AS "ApartmentId",
    a."Name",
    COUNT(b."Id") AS "BookingCount"
FROM "Apartments" a
         LEFT JOIN "Bookings" b ON b."ApartmentId" = a."Id"
GROUP BY a."Id", a."Name"
HAVING COUNT(b."Id") > 0
ORDER BY "BookingCount" DESC
    LIMIT 10;