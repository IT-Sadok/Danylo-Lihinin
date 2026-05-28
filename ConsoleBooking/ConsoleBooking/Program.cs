using ConsoleBooking.Services;
using ConsoleBooking.Models;
using ConsoleBooking.UI;
using ConsoleBooking.Data;
using ConsoleBooking.Data.Repositories;

var repository = new JsonHostRepository();
var hostManager = new HostManager(repository);
var ui = new ConsoleUI(hostManager);

ui.Run();