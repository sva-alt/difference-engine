// See https://aka.ms/new-console-template for more information
using System.Data;

Console.WriteLine("Hello, World!");


string poly = "2x^2 - 3x + 2";
double h = 1;
var machine = new DifferenceMachine(poly, h);

class DifferenceMachine
{
    private double[] polynomial;
    private double h;
    private Dictionary<int,double[]> table;
    private double max_px;
    private double min_px;
    private double last_col_const;
    private int order;

    public DifferenceMachine(string polynomial, double h)
    {
        
        this.h = h;
        this.polynomial = ReadPolynomial(polynomial);
        this.table = CreateTable();


    }

    public double[] ReadPolynomial(string polynomial)
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
        this.order = order+1;
        double[] d_poly_array = new double[order+1];
        for (int i = 0; i < order; i++)
        {
            d_poly_array[i] = 0;
        }

        for (int i = 0; i < poly_array.Length; i++)
        {
            int temp_order = 0;
            double temp_num;
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
                temp_num = System.Convert.ToDouble(temp_poly[0]);
            }
            if (temp_poly.Length > 1)
            {
                temp_order = System.Convert.ToInt16(temp_poly[1]);
            }
            d_poly_array[temp_order] = temp_num;
        }
        return d_poly_array;
    }

    public Dictionary<int,double[]> CreateTable(){
        //TODO: data population
        Dictionary<int, double[]> table = new Dictionary<int, double[]>(); 
        double num = 0;
        int complete_columns = 3;

        for (int i = 0; i < this.order+complete_columns-1; i++) 
        {
            double x = 0;
            double[] temp_array = new double[this.order];
            for (int j = 0; j < this.order; j++)
            {
                if(j == 0)
                {
                    x += this.polynomial[j];
                }
                else
                {
                    x += this.polynomial[j] * Math.Pow(num, Convert.ToDouble(j));
                }
            }
            temp_array[0] = x;
            table[i] = temp_array;
            num += this.h;
        }
        int l = 1;
        for (int i = 1; i < this.order; i++)
        {
            for (int j = 0; j < this.order+complete_columns-l-1; j++)
            {
                table[j][i] = table[j+1][i-1] - table[j][i-1];
            }
            l++;
        }
        this.min_px = table[0][0];
        this.max_px = table[this.order + complete_columns - 1][0];
        this.last_col_const = this.table[0][this.order];

        return table;
    }

    public void CountUp()
    {
        return;
    }

}