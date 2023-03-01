using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Physics.Particles.Contacts
{
    internal class ParticleRod : ParticleLink
    {
        public float Length { get; set; }

        public override ParticleContact FillContact(uint limit)
        {
            float currentLength = CurrentLength();
            if (currentLength == Length)
            {
                return null;
            }
            ParticleContact contact = new ParticleContact();
            contact.Particle1 = Particle1;
            contact.Particle2 = Particle2;

            Vector3 normal = Vector3.Normalize(Particle2.Position - Particle1.Position);
            if (currentLength > Length)
            {
                contact.ContactNormal = normal;
                contact.Penetration = currentLength - Length;
            }
            else
            {
                contact.ContactNormal = -normal;
                contact.Penetration = Length - currentLength;
            }
            contact.Restitution = 0;

            return contact;

        }
    }
}
