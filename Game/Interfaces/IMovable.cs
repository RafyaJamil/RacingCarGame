using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Reflection;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Game.Interfaces
{
    public interface IMovable
    {
        // Velocity of the object
        PointF Velocity { get; set; }
        bool HasPhysics { get; set; }
        float? CustomGravity { get; set; }
    }
}
