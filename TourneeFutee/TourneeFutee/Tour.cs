namespace TourneeFutee
{
    // Modélise une tournée dans le cadre du problème du voyageur de commerce
    public class Tour
    {
        // TODO : ajouter tous les attributs que vous jugerez pertinents 
        float cost;
        int nbSegments;
        List<(string source, string destination)> parcour;

        public Tour ()
        {
            this.cost = 0;
            this.nbSegments = 0;
            this.parcour = new List<(string source, string destination)>();
        }
        
        public Tour(float cost, int nbSegments)
        {
            this.cost = cost;
            this.nbSegments = nbSegments;
            this.parcour = new List<(string source, string destination)>();
        }
        public Tour(List<(string source, string destination)> parcour, float cost)
        {
            this.cost = cost;
            this.nbSegments = parcour.Count;
            this.parcour = new List<(string source, string destination)>(parcour);
        }

        // propriétés
        public float Cost // Coût total de la tournée
        {
            get { return cost;}
            // pas de set
        }
        public int NbSegments // Nombre de trajets dans la tournée
        {
            get { return parcour.Count;}
            // pas de set
        }
        public List<(string source, string destination)> Parcour
        {
            get { return parcour; }
            set { this.parcour = value; }
        }

        // Renvoie vrai si la tournée contient le trajet `source`->`destination`
        public bool ContainsSegment((string source, string destination) segment)
        {
            if(parcour.Count == 0) 
                return false;
            foreach (var s in this.parcour)
            {
                Console.WriteLine(s.source + " --> " + s.destination);
                if((s.source==segment.source)&&(s.destination==segment.destination))
                    return true;
            }
            return false;   
        }

        // Affiche les informations sur la tournée : coût total et trajets
        public void Print()
        {
            if ((this == null) || (this.parcour.Count == 0))
                Console.WriteLine("Tournée inexistante ou vide");
            Console.WriteLine("Coût total : " + this.cost);
            Console.WriteLine("Trajets : ");
            foreach (var segment in this.parcour)
            {
                Console.WriteLine(segment.source+" --> "+segment.destination);
            }
        }
    }
}
