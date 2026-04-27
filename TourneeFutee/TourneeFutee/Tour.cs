namespace TourneeFutee
{
    // Modélise une tournée dans le cadre du problème du voyageur de commerce
    public class Tour
    {
        // TODO : ajouter tous les attributs que vous jugerez pertinents 
        float cost;
        int nbSegments;
        List<(string source, string destination)> parcours;
        List<Sommet> sommets;
        Dictionary<string, int> nomSommets; //associe le nom des sommets à leur indice dans la liste de sommets

        public Tour ()
        {
            this.cost = 0;
            this.nbSegments = 0;
            this.parcours = new List<(string source, string destination)>();
            this.sommets=new List<Sommet> ();
            this.nomSommets=new Dictionary<string, int> ();
        }
        
        public Tour(float cost, int nbSegments)
        {
            this.cost = cost;
            this.nbSegments = nbSegments;
            this.parcours = new List<(string source, string destination)>();
            this.sommets = new List<Sommet>();
            this.nomSommets = new Dictionary<string, int>();
        }
        public Tour(List<(string source, string destination)> parcour, float cost)
        {
            this.cost = cost;
            this.nbSegments = parcour.Count;
            this.parcours = new List<(string source, string destination)>(parcour);
            this.sommets = new List<Sommet>();
            this.nomSommets = new Dictionary<string, int>();
        }
        public Tour(List<string> vertices, float cost)
        {
            this.cost = cost;
            this.parcours = new List<(string source, string destination)>();
            if (vertices != null && vertices.Count >= 2)
            {
                for (int i = 0; i < vertices.Count - 1; i++)
                {
                    this.parcours.Add((vertices[i], vertices[i + 1]));
                }
            }
            this.nbSegments = this.parcours.Count;
            this.sommets = new List<Sommet>();
            this.nomSommets = new Dictionary<string, int>();
        }

        // propriétés
        public float Cost // Coût total de la tournée
        {
            get { return cost;}
            // pas de set
        }
        public int NbSegments // Nombre de trajets dans la tournée
        {
            get { return parcours.Count;}
            // pas de set
        }
        public List<(string source, string destination)> Parcours
        {
            get { return parcours; }
            set { this.parcours = value; }
        }
        public List<Sommet> Sommets
        {
            get{ return sommets; }
            set { sommets = value; }
        }
        public Dictionary<string, int> NomSommets
        {
            get { return nomSommets;}
            set { nomSommets = value; }
        }
        public IList<string> Vertices
        {
            get
            {
                List<string> result = new List<string>();
                if (parcours == null || parcours.Count == 0)
                    return result;
                result.Add(parcours[0].source);
                foreach (var segment in parcours)
                {
                    result.Add(segment.destination);
                }
                return result;
            }
        }

        // Renvoie vrai si la tournée contient le trajet `source`->`destination`
        public bool ContainsSegment((string source, string destination) segment)
        {
            if(parcours.Count == 0) 
                return false;
            foreach (var s in this.parcours)
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
            if ((this == null) || (this.parcours.Count == 0))
                Console.WriteLine("Tournée inexistante ou vide");
            Console.WriteLine("Coût total : " + this.cost);
            Console.WriteLine("Trajets : ");
            foreach (var segment in this.parcours)
            {
                Console.WriteLine(segment.source+" --> "+segment.destination);
            }
        }
    }
}

