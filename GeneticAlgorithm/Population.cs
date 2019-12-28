using System;
using System.Collections.Generic;
using ColorChanges;
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
