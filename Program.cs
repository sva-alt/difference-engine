// See https://aka.ms/new-console-template for more information
using System.Data;

Console.WriteLine("Hello, World!");


string poly = "5x^4 + x + 1";
double h = 0.1;
var machine = new DifferenceMachine(poly, h);

class DifferenceMachine
{
    private decimal[] polynomial;
    private double h;
    private DataTable table;

    public DifferenceMachine(string polynomial, double h)
    {
        
        this.h = h;
        this.polynomial = readPolynomial(polynomial);
        this.table = createTable();


    }



    public decimal[] readPolynomial(string polynomial)
    {
        polynomial = polynomial.Trim();
        for (int i = 1; i < polynomial.Length; i++)
        {
            if (polynomial[i]==' ' && (polynomial[i-1] == '+' || polynomial[i-1] == '-'))
            {
                polynomial = polynomial.Remove(i, 1);
            }
        }

        string[] poly_array = polynomial.Split(' ');
        string[] first_order_string = poly_array[0].Split('^');
        int order = System.Convert.ToInt16(first_order_string[1]);
        decimal[] d_poly_array = new decimal[order+1];
        for (int i = 0; i < order; i++)
        {
            d_poly_array[i] = 0;
        }

        for (int i = 0; i < poly_array.Length; i++)
        {
            int temp_order = 0;
            decimal temp_num;
            poly_array[i] = poly_array[i].Replace("+", "").Replace("^", " ");
            string[] temp_poly = poly_array[i].Split();
            if (temp_poly[0].Contains('x') && temp_poly.Length == 1)
            {
                temp_order = 1;
            }
            temp_poly[0] = temp_poly[0].Replace("x", "");
            if (temp_poly[0] == "")
            {
                temp_num = 1;

            } 
            else
            {
                temp_num = System.Convert.ToDecimal(temp_poly[0]);
            }
            if (temp_poly.Length > 1)
            {
                temp_order = System.Convert.ToInt16(temp_poly[1]);
            }
            d_poly_array[temp_order] = temp_num;
        }


        return d_poly_array;
    }

    public DataTable createTable(){
        DataTable piv = new DataTable("Polynomial_initial_values");






        return piv;

    }
}