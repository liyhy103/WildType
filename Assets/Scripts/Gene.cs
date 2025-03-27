using UnityEngine;

public class Gene
{
    // Name of the trait this gene controls (e.g., "Coat Color")
    public string TraitName;

    // Two alleles for this gene (e.g., 'B' and 'b')
    public char Allele1;
    public char Allele2;

    public Gene(string traitName, char allele1, char allele2){
        this.TraitName = traitName;
        this.Allele1 = allele1;
        this.Allele2 = allele2;
    }

    // Returns the genotype as a string (e.g., "B/b")
    public string GetGenotype(){
        return $"{Allele1} / {Allele2}";
    }

    // Returns the phenotype based on genotype (for lesson 1 Mendelian)
    public string GetPhenotype(){
        // B is dominant (blue coat), b is recessive (red coat)
        if(Allele1 == 'B' || Allele2 == 'B')
            return "Blue";
        else
            return "Red";
    }
}
