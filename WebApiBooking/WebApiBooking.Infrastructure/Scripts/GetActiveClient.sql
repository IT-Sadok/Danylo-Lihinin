SELECT
    b."UserId",
    u."Name" AS "Name",
    COUNT(*) AS "TotalBookings",
    SUM(a."Price" * EXTRACT(EPOCH FROM (b."EndDate" - b."StartDate")) / 86400.0) AS "TotalSpent"
FROM "Bookings" b
         JOIN "AspNetUsers" u ON u."Id" = b."UserId"
         JOIN "Apartments" a ON a."Id" = b."ApartmentId"
GROUP BY b."UserId", u."Name"
HAVING COUNT(*) > 1
ORDER BY "TotalSpent" DESC;