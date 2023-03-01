using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Physics.Particles;

namespace Physics.Particles.Generators
{
    public interface ForceGenerator
    {
        public Vector3 Update(Data data, float duration);
    }
}
