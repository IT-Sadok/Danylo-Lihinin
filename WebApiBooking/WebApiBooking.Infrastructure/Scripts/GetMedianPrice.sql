SELECT
    a."HostId",
    u."Name"                                                  AS "HostName",
    COUNT(*)                                                   AS "ApartmentCount",
    PERCENTILE_CONT(0.5) WITHIN GROUP (ORDER BY a."Price")     AS "MedianPrice",
    PERCENTILE_CONT(0.9) WITHIN GROUP (ORDER BY a."Price")     AS "P90Price"
FROM "Apartments" a
    JOIN "AspNetUsers" u ON u."Id" = a."HostId"
GROUP BY a."HostId", u."Name"
HAVING COUNT(*) >= 2
ORDER BY "MedianPrice" DESC;