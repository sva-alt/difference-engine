// See https://aka.ms/new-console-template for more information
using System.Data;
using System.Globalization;

Console.WriteLine("Hello, World!");


string poly = "2x^2 - 3x + 2";
double h = 1;
double x = -1;
var machine = new DifferenceMachine(poly, h, x);

class DifferenceMachine
{
    private double[] polynomial;
    private double h;
    private double x;
    private Dictionary<int,double[]> table;
    private double max_px;
    private double max_diff;
    private int max_index;
    private double min_px;
    private double min_diff;
    private double last_col_const;
    private int complete_columns;
    private int order;

    public DifferenceMachine(string polynomial, double h, double x)
    {
        
        this.h = h;
        this.x = x;
        this.polynomial = ReadPolynomial(polynomial);
        this.table = CreateTable();

        if (x > Math.Abs(this.h * this.max_index))
        {
            // CountUp cycle
            int steps = Convert.ToInt16(Math.Abs((this.x/h)- this.max_index));
            for (int i = 0; i < steps; i++)
            {
                CountUp();
            }
            GiveResult(polynomial, max_px);
        }
        else if(x < 0)
        {
            // CountDown cycle
            int steps = Convert.ToInt16(Math.Abs(this.x/h));
            for (int i = 0; i < steps; i++)
            {
                CountDown();
            }
            GiveResult(polynomial, min_px);
        } else
        {
            // Check answers in table
            Console.WriteLine("Beware, answer is between generated table, " + 
            "if h is not small enough, precision will be bad!");
            double[] array_px = GetAllx();
            double min_num = Math.Abs(this.x-(this.h * 0));
            int index_min_num = 0;
            for (int i = 1; i < array_px.Length; i++)
            {
                double temp = Math.Abs(this.x-(this.h * i));
                if (temp < min_num)
                {
                    min_num = temp;
                    index_min_num = i;
                }
            }
            GiveResult(polynomial, array_px[index_min_num]);
        }
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
        this.order = order;
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
        this.complete_columns = 3;
        this.max_index = this.order+this.complete_columns - 1;

        for (int i = 0; i < this.max_index+1; i++) 
        {
            double x = 0;
            double[] temp_array = new double[this.order+1];
            for (int j = 0; j < this.order+1; j++)
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
        for (int i = 1; i < this.order+1; i++)
        {
            for (int j = 0; j < this.max_index+1-l; j++)
            {
                table[j][i] = table[j+1][i-1] - table[j][i-1];
            }
            l++;
        }
        this.min_px = table[0][0];
        this.min_diff = table[0][1];
        this.max_px = table[this.max_index][0];
        this.max_diff = table[this.max_index - 1][1];
        this.last_col_const = table[0][this.order];

        return table;
    }



    public void CountUp()
    {
        this.max_diff += this.last_col_const;
        this.max_px += this.max_diff;
        return;
    }



    public void CountDown()
    {
        this.min_diff -= this.last_col_const;
        this.min_px -= this.min_diff;
        return;
    }



    public double[] GetAllx()
    {
        double[] array_px = new double[this.max_index+1];
        for (int i = 0; i < this.max_index+1; i++)
        {
            array_px[i] = table[i][0];
        }
        return array_px;
    }



    public void GiveResult(string raw_poly, double num)
    {
        Console.WriteLine(String.Format("the polynomial {0}, where x is {1}, is equal to {2}", raw_poly, this.x, num));
    }

}