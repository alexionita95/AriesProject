using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Physics.Particles.Contacts
{
    public abstract class ParticleLink
    {
        public Particle Particle1 { get; set; }
        public Particle? Particle2 { get; set; }


        public float CurrentLength()
        {
            Vector3 relative = Particle1.Position - Particle2.Position;
            return relative.Length();
        }
        public abstract ParticleContact FillContact(uint limit);
    }
}
