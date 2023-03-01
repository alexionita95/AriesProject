using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Physics.Particles.Generators
{
    public class GravityGenerator : ForceGenerator
    {
        public Vector3 Gravity { get; set; }
        public Vector3 Update(Data data, float duration)
        {
            if (data.InfiniteMass)
            {
                return Vector3.Zero;
            }
            return Gravity * data.Mass;
        }
    }
}
