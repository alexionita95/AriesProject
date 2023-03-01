using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.Particles.Contacts
{
    public class ParticleContactResolver
    {
        public int Iterations { get; set; }

        int iterationsUsed;

        public void ResolveContacts(List<ParticleContact> contacts, float duration)
        {
            float max = 0;
            iterationsUsed = 0;
            while (iterationsUsed < Iterations)
            {
                int maxIndex = contacts.Count;
                for (int i = 0; i < contacts.Count; ++i)
                {
                    float sepVel = contacts[i].CalculateSeparatingVelocity();
                    if (sepVel > max)
                    {
                        max = sepVel;
                        maxIndex = i;
                    }
                }
                contacts[maxIndex].Resolve(duration);
                iterationsUsed++;

            }
        }
    }
}
