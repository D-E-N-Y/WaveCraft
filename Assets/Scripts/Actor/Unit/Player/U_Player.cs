
using UnityEngine;

public class U_Player : Unit
{
    public string proffesion { get; protected set; }
    private string[] names = {"Coll Ó Broin", "Searc Macdougalls", "Guirmean Duncansons", "Carmag Mcmahoni", "Sionn Fergusoni",
                            "Gòrdan O'Bernei", "Finnean Jewele", "Eoghanan Ó Faoláini", "Conn Ó Nualláini", "Dùghlas Mcgeei",
                            "Madadh Donohuei", "Cathalan Keegan", "Lulach Finnegani", "Brianan Ó Conchobhairi", "Conall O'Connor",
                            "Dùghlas Guinness", "Garbhan Ó Riagáin", "Aodhan Mahoneyi", "Colla Ó Fionnáini", "Fionntan Maccances",
                            "Fionghan Fannoni", "Gilleathain Ó Faoláini", "Fionn Whaleni", "Cosgrach Ó Briain", "Dubh Ó Broin"};

    public override void Initialize()
    {
        base.Initialize();

        nameActor = names[Random.Range(0, names.Length)];

        VillageSystem.current.AddVillage(this);
    }
}