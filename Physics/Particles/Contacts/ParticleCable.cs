using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Physics.Particles.Contacts
{
    public class ParticleCable : ParticleLink
    {

        public float Restitution { get; set; }
        public float MaxLength { get; set; }

        public override ParticleContact FillContact(uint limit)
        {
            float length = CurrentLength();
            if (length < MaxLength)
            {
                return null;
            }
            ParticleContact contact = new ParticleContact();

            contact.Particle1 = Particle1;
            contact.Particle2 = Particle2;
            Vector3 normal = Vector3.Normalize(Particle2.Position - Particle1.Position);
            contact.ContactNormal = normal;
            contact.Penetration = length - MaxLength;
            contact.Restitution = Restitution;
            return contact;
        }
    }
}
