namespace Domain
{
    public class Population
    {
        // TODO: make this property private
        public List<List<Individual>> Areas = new List<List<Individual>>();

        public event EventHandler onBornIndividual;

        public event EventHandler onDieIndividual;


        // TODO: make it parametrized depended on count of areas
        public Population()
        {
            Areas.Add(new List<Individual>());
            Areas.Add(new List<Individual>());
            Areas.Add(new List<Individual>());
            Areas.Add(new List<Individual>());
            Areas.Add(new List<Individual>());
            Areas.Add(new List<Individual>());
        }

        public void AddIndividual(int areaCode, Individual individual)
        {
            if (areaCode > Areas.Count || areaCode < 0)
            {
                return;
            }

            Areas[areaCode].Add(individual);
            onBornIndividual?.Invoke(this, EventArgs.Empty);
        }

        public void RemoveIndividual(int areaCode, Individual individual)
        {
            if (areaCode > Areas.Count || areaCode < 0)
            {
                return;
            }

            Areas[areaCode].Remove(individual);
            onDieIndividual?.Invoke(this, EventArgs.Empty);
        }
    }
}
