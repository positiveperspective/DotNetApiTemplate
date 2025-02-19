using DotNetAPI.Domain.Common.Interfaces;

namespace DotNetAPI.Core;

internal class MachineDateTime : IDateTime
{
    public DateTime Current
    {
        get
        {
            DateTime currentDate = DateTime.Now;
            return new DateTime(currentDate.Ticks - (currentDate.Ticks % TimeSpan.TicksPerSecond), currentDate.Kind);
        }
    }
}
