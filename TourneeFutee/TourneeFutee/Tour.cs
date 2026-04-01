namespace TourneeFutee
{
    // Modélise une tournée dans le cadre du problème du voyageur de commerce
    public class Tour
    {
        // TODO : ajouter tous les attributs que vous jugerez pertinents 
        float cost;
        int nbSegments;
        List<(string source, string destination)> parcour;

        public Tour (float cost, int nbSegments)
        {
            this.cost = cost;
            this.nbSegments = nbSegments;
            this.parcour=new List<(string source, string destination)> ();
        }
        public Tour ()
        {
            this.cost = 0;
            this.nbSegments = 0;
            this.parcour = new List<(string source, string destination)>();
        }

        // propriétés

        // Coût total de la tournée
        public float Cost
        {
            get;    // TODO : implémenter
        }

        // Nombre de trajets dans la tournée
        public int NbSegments
        {
            get;    // TODO : implémenter
        }
        public List<(string source, string destination)> Parcour
        {
            get { return parcour; }
            set { this.parcour = value; }
        }


        // Renvoie vrai si la tournée contient le trajet `source`->`destination`
        public bool ContainsSegment((string source, string destination) segment)
        {
            return false;   // TODO : implémenter 
        }


        // Affiche les informations sur la tournée : coût total et trajets
        public void Print()
        {
            // TODO : implémenter 
        }

        // TODO : ajouter toutes les méthodes que vous jugerez pertinentes 

    }
}
