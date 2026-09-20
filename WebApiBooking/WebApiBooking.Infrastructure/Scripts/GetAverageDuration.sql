SELECT
    a."Id"   AS "ApartmentId",
    a."Name",
    AVG(b."EndDate" - b."StartDate") AS "Duration",
    AVG(a."Price" * EXTRACT(EPOCH FROM (b."EndDate" - b."StartDate")) / 86400.0) AS "Income"
FROM "Bookings" b
         JOIN "Apartments" a ON a."Id" = b."ApartmentId"
GROUP BY a."Id", a."Name";