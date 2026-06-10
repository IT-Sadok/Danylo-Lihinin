using ConsoleBooking.Services;
using ConsoleBooking.Models;
using ConsoleBooking.UI;
using ConsoleBooking.Data;
using ConsoleBooking.Data.Repositories;
using ConsoleBooking.Services.Mapping;

var repository = new JsonHostRepository();
var mapper = new HostMapper();
var hostManager = new HostService(repository,mapper);
var ui = new ConsoleUI(hostManager);

ui.Run();