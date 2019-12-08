using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithm
{
    public class Population
    {
        public List<List<Individual>> Areas = new List<List<Individual>>();
        public Population()
        {
            Areas.Add(new List<Individual>());
            Areas.Add(new List<Individual>());
            Areas.Add(new List<Individual>());
            Areas.Add(new List<Individual>());
            Areas.Add(new List<Individual>());
            Areas.Add(new List<Individual>());
        }
    }
}
