using ConsoleBooking.Services;
using ConsoleBooking.Models;
using ConsoleBooking.UI;
using ConsoleBooking.Data;
using ConsoleBooking.Data.Repositories;
using ConsoleBooking.RaceCondtionSimulation;
using ConsoleBooking.Services.Mapping;

var repository = new JsonHostRepository();
var mapper = new HostMapper();
RaceConditionSimulation sim = new RaceConditionSimulation
{
HostRepository =  repository,
};
var hostManager = new HostService(repository,mapper,sim);
var ui = new ConsoleUI(hostManager);
ui.Run();