INSERT INTO "Apartments" ("Id", "Name", "Price", "Rooms", "CustomData", "HostId")
VALUES (
           COALESCE(@Id, nextval(pg_get_serial_sequence('"Apartments"', 'Id'))),
           @Name, @Price, @Rooms, @CustomData::jsonb, @HostId
       )
    ON CONFLICT ("Id") DO UPDATE SET
    "Name" = EXCLUDED."Name",
                              "Price" = EXCLUDED."Price",
                              "Rooms" = EXCLUDED."Rooms",
                              "CustomData" = EXCLUDED."CustomData"
                              RETURNING "Id";