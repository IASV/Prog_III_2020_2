﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace Prog_III_2020_2_sesion_1
{
    class Venta
    {
        public static List<Venta> ListaVentas;
        public Cliente Client { get; set; }
        public Vendedor Vend { get; set; }
        public Inventario Item { get; set; }
        public int IdVenta { get; set; }

        public void Add()
        {
            if (ListaVentas == null)
            {
                ListaVentas = new List<Venta>();
            }

            ListaVentas.Add(this);

            Save();
        }

        private void Save()
        {
            StreamWriter writer = new StreamWriter("Files/Venta.txt", true);

            writer.WriteLine(Client.ToString() + "," + Vend.ToString() + "," + Item.ToString() + "," + IdVenta.ToString());

            writer.Close();
        }

        public void Delete()
        {
            if (Find(this.IdVenta))
            {
                ListaVentas.Remove(this);
            }
        }
        public static void Delete(object data)
        {
            using (StreamWriter fileWrite = new StreamWriter("Files/temp.txt", true))
            {
                using (StreamReader fielRead = new StreamReader("Files/Venta.txt"))
                {
                    String line;

                    while ((line = fielRead.ReadLine()) != null)
                    {
                        string[] datos = line.Split(new char[] { ',' });
                        string[] dateValues = (data.ToString()).Split('\t');
                        if (datos[0].ToString() != dateValues[0].ToString())
                        {
                            fileWrite.WriteLine(line);
                        }

                    }
                }
            }

            //aqui se renombrea el archivo temporal
            File.Delete("Files/Venta.txt");
            File.Move("Files/temp.txt", "Files/Venta.txt");
        }
        public static void Edit(int linea, int i, object data, string Archivo)
        {
            string[] All = File.ReadAllLines(Archivo);
            string[] Lines = (All[linea]).Split(',');
            string[] date = (data.ToString()).Split('\t');
            Lines[i] = date[i];
            string dataText = "";
            for (int j = 0; j < Lines.Length; j++)
            {
                dataText += Lines[j];
                if (j < Lines.Length) dataText += ",";
            }

            All[linea] = dataText;

            File.WriteAllLines(Archivo, All);
        }
        public static Venta Parse(string value)
        {
            Venta v = new Venta();

            string[] values = value.Split('\t');

            if (values[0] != "")
            {
                v.Client = Cliente.Parse(values[0]);

                v.Vend = Vendedor.Parse(values[1]);

                v.Item = Inventario.Parse(values[2]);

                v.IdVenta = Convert.ToInt32(values[3]);
            }
            return v;
        }

        public static void Update(int IdVenta, int NDato)
        {

            if (Find(IdVenta))
            {
                Venta v = Search(IdVenta);
                if (v != null)
                {
                    switch (NDato)
                    {
                        case 1:
                            
                            Console.WriteLine("\nNuevo Cliente: ");

                            Console.Write("Ingrese cédula del cliente: ");
                            long cedCliente = Scanner.NextLong();
                            if (Cliente.Find(cedCliente))
                            {
                                v.Client = Cliente.Search(cedCliente);
                            }
                            else
                            {
                                Console.WriteLine("Cliente no encontrado o no registrado.\nIngrese al menú crear cliente.");
                                Cliente.MenuClientes();
                                if (Cliente.Find(cedCliente))
                                {
                                    v.Client = Cliente.Search(cedCliente);
                                }
                                else Console.WriteLine("¡Ooops, Cliente no creado!");
                            }
                            break;

                        case 2:
                            NDato = 3;
                            Console.Write("\nNuevo Item: ");

                            Console.WriteLine(" --- Lista de Carros ---\n-- Seleccione el Carro --");
                            Inventario.CarsShowItems();
                            Console.Write("Ingrese el ID del Carro: ");
                            int IdCar = Scanner.NextInt();

                            Inventario item = Inventario.SearchForCar(IdCar);

                            if (Inventario.Find(item.IdInventario))
                            {
                                v.Item = item;
                            }

                            Console.Write("Ingrese fecha de venta. dd/MM/yyyy\n:: ");
                            v.Item.FechaSalida = DateTime.Parse(Scanner.NextLine());
                            break;

                    }

                    Edit(ListaVentas.IndexOf(v), NDato - 1, v, "Files/Venta.txt");
                }
                else Console.WriteLine("¡Oooops, A ocurrido un erro!");
            }

        }

        /// <summary>
        /// Muestra los datos de todos los Ventaes
        /// </summary>

        public static void ToList()
        {

            foreach (Venta v in ListaVentas)
            {
                v.Show();
                //Console.WriteLine(v.ToString());
            }
        }

        //public string List()
        //{
        //    string todos = "";
        //    foreach (Venta Venta in ListaVentas)
        //    {
        //        todos += Venta.ToString();
        //    }
        //    return todos;
        //}

        /// <summary>
        /// Muestra los datos de un Venta
        /// </summary>
        public void Show()
        {
            Console.WriteLine(Client.ToString().PadRight(5) + Vend.ToString().PadLeft(2).PadRight(4) 
                + Item.ToString().PadRight(10) + IdVenta.ToString().PadRight(2).PadLeft(4));
        }

        public override string ToString()
        {
            return (Client.ToString() + "\t" + Vend.ToString() + "\t" + Item.ToString() + "\t" + IdVenta.ToString());
        }

        public static bool Find(int IdVenta)
        {
            foreach (Venta Venta in ListaVentas)
            {
                if (Venta.IdVenta == IdVenta) return true;
            }
            return false;
        }

        public static Venta Search(int IdVenta)
        {
            foreach (Venta v in ListaVentas)
            {
                if (v.IdVenta == IdVenta)
                {
                    return v;
                }
            }
            return null;
        }

        public static Venta Search(Cliente cliente)
        {
            foreach (Venta v in ListaVentas)
            {
                if (v.Client == cliente)
                {
                    return v;
                }
            }
            return null;
        }

        public void SetItems(int i, string value)
        {
            switch (i)
            {
                case 0:
                    Client = Cliente.Parse(value);
                    break;
                case 1:
                    
                    Vend = Vendedor.Parse(value);
                    break;
                case 2:
                    Item = Inventario.Parse(value);
                    
                    break;
                case 3:
                    IdVenta = Convert.ToInt32(value);
                    break;
            }
        }

        public static void LoadList()
        {
            ListaVentas = new List<Venta>();
            if (File.Exists("Files/Venta.txt"))
            {
                StreamReader reader = new StreamReader("Files/Venta.txt");

                while (!reader.EndOfStream)
                {
                    string[] var = reader.ReadLine().Split(',');

                    Venta v = new Venta();
                    for (int i = 0; i < var.Length; i++)
                    {
                        v.SetItems(i, var[i]);
                    }

                    ListaVentas.Add(v);

                }

                reader.Close();
            }
        }

        public static void MenuVenta()
        {
            int option;

            Vendedor.LoadList();
            Cliente.LoadList();
            Carro.LoadList();
            Inventario.LoadList();
            LoadList();
            
            do
            {
                Console.Write("\n\tBienvenido al menú de Ventas\n" +
                    "\t1. Crear Venta.\n" +
                    "\t2. Eliminar Venta.\n" +
                    "\t3. Editar Venta.\n" +
                    //"\t4. Listar Ventas.\n" +
                    "\t5. Buscar Venta.\n" +
                    "\t6. Salir.\n" +
                    "\t:: ");

                option = Scanner.NextInt();

                switch (option)
                {
                    case 1:
                        Console.Clear();
                        Console.WriteLine("\n\t-- Crear Venta ---");
                        Venta v = new Venta();

                        Console.Write("Ingrese cédula del cliente: ");
                        long cedCliente = Scanner.NextLong();
                        if (Cliente.Find(cedCliente))
                        {
                            v.Client = Cliente.Search(cedCliente);
                        }
                        else
                        {
                            Console.WriteLine("Cliente no encontrado o no registrado.\nIngresando al menú cliente para crear cliente.");
                            Cliente.CreateCliente();
                            if (Cliente.Find(cedCliente))
                            {
                                v.Client = Cliente.Search(cedCliente);
                            }
                            else
                            {
                                Console.WriteLine("¡Ooops, Cliente no creado!");
                                break;
                            }
                        }
                        bool condition = false;
                        do
                        {
                            Console.Write("Ingrese cédula del vendedor que realizo la venta\n:: ");
                            long cedVendedor = Scanner.NextLong();

                            if (Vendedor.Find(cedVendedor))
                            {
                                v.Vend = Vendedor.Search(cedVendedor);
                                condition = true;
                            }
                            else
                            {
                                Console.WriteLine("\n¡Ooops, Vendedor no encontrado!\n");
                            }
                        } while (condition != true);

                        Console.WriteLine(" --- Lista de Carros ---\n-- Seleccione el Carro --");
                        Inventario.CarsShowItems();
                        Console.Write("Ingrese el ID del Carro: ");
                        int IdCar = Scanner.NextInt();

                        Inventario item = Inventario.SearchForCar(IdCar);

                        if (Inventario.Find(item.IdInventario))
                        {
                            v.Item = item;
                        }

                        Console.Write("Ingrese fecha de venta. dd/MM/yyyy\n:: ");
                        v.Item.FechaSalida = DateTime.Parse(Scanner.NextLine());

                        if (ListaVentas.Count != 0)
                            v.IdVenta = ListaVentas.Last().IdVenta + 1;
                        else
                            v.IdVenta = 1;

                        v.Show();

                        v.Add();

                        break;

                    case 2:
                        Console.Clear();
                        Console.Write("\n\t--- Eliminar Venta ---\nNúmero de ID de la venta: ");
                        int venta = Scanner.NextInt();

                        if (Find(venta))
                        {
                            Venta vn = Search(venta);
                            vn.Show();
                            Console.Write("\n¿Borrar Venta?\n\t1. Si.\n\t2. No.\n:: ");
                            if (Scanner.NextInt() == 1)
                            {
                                vn.Delete();
                                Delete(vn);
                                Console.WriteLine("\n¡Proceso realizado con éxito!");
                            }
                            else Console.WriteLine("\n¡Proceso cancelado!");

                        }

                        break;
                    case 3:
                        Console.Clear();
                        Console.Write("\n\t--- Editar datos de la Venta ---\nNúmero de ID de la venta: ");
                        int IdItem = Scanner.NextInt();
                        Search(IdItem).Show();
                        Console.Write("\n\tOpciones a editar:\n" +
                           "\t1.  Nuevo Cliente.\n" +
                           "\t2.  Nuevo Item.\n" +
                           "\t:: ");

                        Update(IdItem, Scanner.NextInt());
                        break;
                    case 4:
                        Console.Clear();
                        Console.WriteLine("\n\t-- Lista de Ventas ---\n");
                        ToList();
                        break;

                    case 5:
                        Console.Clear();
                        Console.WriteLine("\n\t-- Buscar Venta ---\n");

                        Console.Write("\nIngrese el ID de la Venta.\n:: ");
                        Search(Scanner.NextInt()).Show();
                        break;
                }

            } while (option != 6);
        }

    }
}