using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http.Headers;
using System.Reflection.Emit;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
public class Game
{
    #pragma warning disable CS8618
    #pragma warning disable CS8600
    private int n = 25;
    private int canttramp = 5;
    private int canttransp = 5;
    private int restmon = 6;
    private int cantpot = 5;
    private int canthab = 5;
    private int cantmon = 17;
    private int turno = 0;
    private int contador = 0;
    private int rest;
    private int pasos;
    private int puntuacion = 0;
    private int guardar_enfriamiento;
    private int posicion_x;
    private int posicion_y;
    private int ant_posicion_x;
    private int ant_posicion_y;
    private List<string> selection_personaje;
    private List<string> selection_habilidad;
    private List<int> selection_velocidad;
    private List<int> selection_enfriamiento;
    private int control;
    private int[,]coordenadas;
    private int[,]direcciones_x1 = {
        {-1,0},
        {1,0},
        {0,-1},
        {0,1},
        {-1,-1},
        {1,-1},
        {-1,1},
        {1,1}
    };
    private int[,]direcciones_x2 = {
        {-1,0},
        {1,0},
        {0,-1},
        {0,1},
        {-1,-1},
        {1,-1},
        {-1,1},
        {1,1},
        {0,2},
        {2,0},
        {0,-2},
        {-2,0},
        {-2,-1},
        {-1,-2},
        {-2,-2},
        {-2,1},
        {-1,2},
        {-2,2},
        {2,-1},
        {1,-2},
        {2,-2},
        {2,1},
        {1,2},
        {2,2}
    };
    private int[,]direcciones_x3 = {
        {-1,0}, {0,3},
        {1,0}, {1,3},
        {0,-1}, {2,3},
        {0,1}, {3,3},
        {-1,-1}, {3,2},
        {1,-1}, {3,1},
        {-1,1}, {3,0},
        {1,1}, {3,-1},
        {0,2}, {3,-2},
        {2,0}, {3,-3},
        {0,-2}, {2,-3},
        {-2,0}, {1,-3},
        {-2,-1}, {0,-3},
        {-1,-2}, {-1,-3},
        {-2,-2}, {-2,-3},
        {-2,1}, {-3,-3},
        {-1,2}, {-3,-2},
        {-2,2}, {-3,-1},
        {2,-1}, {-3,0},
        {1,-2}, {-3,1}, 
        {2,-2}, {-3,2},
        {2,1}, {-3,3},
        {1,2}, {-2,3},
        {2,2}, {-1,3}
    };
    private string P1_Personaje;
    private string P1_Habilidad;
    private int P1_Velocidad;
    private int P1_Enfriamiento;
    private int P1_Guardar_Enfriamiento;
    private int P1_Posicion_x = 1;
    private int P1_Posicion_y = 1;
    private int P1_Puntuacion = 0;

    private string P2_Personaje;
    private string P2_Habilidad;
    private int P2_Velocidad;
    private int P2_Enfriamiento;
    private int P2_Guardar_Enfriamiento;
    private int P2_Posicion_x = 1;
    private int P2_Posicion_y = 23;
    private int P2_Puntuacion = 0;

    
    private string P3_Personaje;
    private string P3_Habilidad;
    private int P3_Velocidad;
    private int P3_Enfriamiento;
    private int P3_Guardar_Enfriamiento;
    private int P3_Posicion_x = 23;
    private int P3_Posicion_y = 1;
    private int P3_Puntuacion = 0;

    
    private string P4_Personaje;
    private string P4_Habilidad;
    private int P4_Velocidad;
    private int P4_Enfriamiento;
    private int P4_Guardar_Enfriamiento;
    private int P4_Posicion_x = 23;
    private int P4_Posicion_y = 23;
    private int P4_Puntuacion = 0;



    public Game()
    {
        coordenadas = Terreno();
        Generar_Personajes();
        Laberinto();
        Aleatorio_Generar(cantmon, 5, direcciones_x2); //Genera las monedas
        Aleatorio_Trampas(canttramp, 2); //Genera las trampas
        Aleatorio_Trampas(canttransp, 3); //Genera las trampas de teletransporte
        Aleatorio_Generar(cantpot, 4, direcciones_x3); //Genera los potenciadores
        Aleatorio_Generar(canthab, 6, direcciones_x3); //Genera las recargas de habilidad
        Aleatorio_Generar(restmon, 7, direcciones_x3); //Genera los restadores de monedas
    }
    private int[,]Terreno()
    {
        return new int[n, n];
    }
    private void Laberinto()
    {
        for(int y = 0; y < n; y++)
        {
            for(int x = 0; x < n; x++)
            {
                coordenadas[x, y] = 1;
            }
        }
        Generar_Laberinto(1, 1);
    }
    private void Generar_Laberinto(int x, int y)
    {
        int[] direcciones = { 0, 1, 2, 3 }; 
        Random count = new Random();
        Mezclar_Array(direcciones, count);

        foreach (int direccion in direcciones)
        {
            int nuevoX = x, nuevoY = y;

            switch (direccion)
            {
                case 0: nuevoY -= 2; break; 
                case 1: nuevoX += 2; break; 
                case 2: nuevoY += 2; break; 
                case 3: nuevoX -= 2; break; 
            }

            if (Mov_Valido(nuevoX, nuevoY))
            {
                coordenadas[y + (nuevoY - y) / 2, x + (nuevoX - x) / 2] = 0; 
                coordenadas[nuevoY, nuevoX] = 0;
                Generar_Laberinto(nuevoX, nuevoY); 
            }
        }
    }
    private bool Mov_Valido(int x, int y)
    {
        return x > 0 && y > 0 && x < n && y < n && coordenadas[y, x] == 1;
    }
    private void Mezclar_Array(int[] array, Random count)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int j = count.Next(0, i + 1);
            int temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }
    }
    private void Aleatorio_Trampas(int cantidad, int valor)
    {
        Random count = new Random();
        int contador = 0;
        while(contador < cantidad)
        {
            int fila = count.Next(n);
            int columna = count.Next(n);

            if(coordenadas[fila, columna] == 0 && Ubicacion_de_Trampas(fila, columna, valor) && Adjunto(fila, columna))
            {
                coordenadas[fila, columna] = valor;
                contador++;
            }
        }
    }
    private bool Ubicacion_de_Trampas(int fila, int columna, int valor)
    {
        for(int i = 0; i < direcciones_x2.GetLength(0); i++)    
        {
            int newfila = fila + direcciones_x2[i, 0];
            int newcolumna = columna + direcciones_x2[i, 1];
            if(newfila >= 0 && newcolumna  >= 0 && newfila < n && newcolumna < n)
            {
                if(coordenadas[newfila, newcolumna] == valor)
                {
                    return false;
                }
            }
        }
        return true;
    }
    private bool Adjunto(int fila, int columna)
    {
        for(int i = 0; i < direcciones_x1.GetLength(0); i++)    
        {
            int newfila = fila + direcciones_x1[i, 0];
            int newcolumna = columna + direcciones_x1[i, 1];
            if(newfila >= 0 && newcolumna  >= 0 && newfila < n && newcolumna < n)
            {
                if(coordenadas[newfila, newcolumna] == 5)
                {
                    return true;
                }
            }
        }  
        return false;
    }
    private void Aleatorio_Generar(int cantidad, int valor, int[,] direcciones)
    {
        Random count = new Random();
        int contador = 0;
        while(contador < cantidad)
        {
            int fila = count.Next(n);
            int columna = count.Next(n);

            if(coordenadas[fila, columna] == 0 && Ubicacion_de_Generar(fila, columna, valor, direcciones))
            {
                coordenadas[fila, columna] = valor;
                contador++;
            }
        }
    }
    private bool Ubicacion_de_Generar(int fila, int columna, int valor, int[,] direcciones)
    {
        for(int i = 0; i < direcciones.GetLength(0); i++)    
        {
            int newfila = fila + direcciones[i, 0];
            int newcolumna = columna + direcciones[i, 1];
            if(newfila >= 0 && newcolumna  >= 0 && newfila < n && newcolumna < n)
            {
                if(coordenadas[newfila, newcolumna] == valor)
                {
                    return false;
                }
            }
        }  
        return true;
    }
    private void Generar_Personajes()
    {
        if(control >= 2)
        {
            coordenadas[P1_Posicion_x, P1_Posicion_y] = 11;
            coordenadas[P2_Posicion_x, P2_Posicion_y] = 12;

            if(control >= 3)
            {
                coordenadas[P3_Posicion_x, P3_Posicion_y] = 13;

                if(control >= 4)
                {
                    coordenadas[P4_Posicion_x, P4_Posicion_y] = 14;
                }
            }

        }
    } 
    public void Mostrar_Mapa()
    {
        Console.Clear();
        for (int i = 0; i < n; i++)
        {
            for(int j = 0; j < n; j++)
            {
                if(coordenadas[i, j] == 0)//Casilla disponible
                {
                    Console.Write("  ");
                }
                else if(coordenadas[i, j] == 1)//Casilla con obstaculo
                {
                    Console.Write(" ■");
                }
                else if(coordenadas[i, j] == 2)//Casilla con trampa
                {
                    Console.Write("  ");
                }
                else if(coordenadas[i, j] == 3)//Casilla con trampa teletransporte
                {
                    Console.Write("  ");
                }
                else if(coordenadas[i, j] == 7)//Casilla restadora de moneda
                {
                    Console.Write("  ");
                }
                else if(coordenadas[i, j] == 5)//Casilla con moneda
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(" $");
                    Console.ResetColor();
                }
                else if(coordenadas[i, j] == 6)//Casilla con recarga de habilidad
                {
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.Write(" R");
                    Console.ResetColor();
                }
                else if(coordenadas[i, j] == 4)//Casilla con potenciador
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Write(" P");
                    Console.ResetColor();
                }
                else if(coordenadas[i, j] == 11)//Jugador #1
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(" ■");
                    Console.ResetColor();
                } 
                else if(coordenadas[i, j] == 12)//Jugador #2
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write(" ■");
                    Console.ResetColor();
                } 
                else if(coordenadas[i, j] == 13)//Jugador #3 
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(" ■");
                    Console.ResetColor();
                } 
                else if(coordenadas[i, j] == 14)//Jugador #4
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.Write(" ■");
                    Console.ResetColor();
                } 
            }
            Console.WriteLine();
        }
        Console.WriteLine("Instrucciones:");
        Console.Write($"Presione 'W A S D' para moverete.");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("          '$'");
        Console.ResetColor();
        Console.Write(" - Son las casillas de Monedas\n");
        Console.Write("Presione 'E' para activar la habilidad. ");
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.Write("   'P'");
        Console.ResetColor();
        Console.Write(" - Son las casillas de Recarga\n");
        Console.Write("Presione 'Enter' para realizar su acción. ");
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.Write(" 'R'");
        Console.ResetColor();
        Console.Write(" - Son las casillas de Potenciadores");
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.Write("  Cuidado hay trampas ocultas !!!\n");
        Console.ResetColor();
    }
    public void Inicio()
    { 
        Console.Clear();
        Console.Write("\n\n   Bienvenidos a Maze-Runners\n\n"); 
        Console.WriteLine("¡Atención, jugadores!\n");
        Console.WriteLine("Cuidado hay trampas ocultas en cada rincón, así que mantente alerta.");
        Console.WriteLine("Recoge las monedas lo más rápido que puedas.");
        Console.WriteLine("El jugador que más monedas logre obtener será el ganador.");
        Console.WriteLine("¡Mucha suerte a todos!.\n");

        Console.WriteLine("Presione 'W A S D' para moverete.");
        Console.WriteLine("Presione 'E' para activar la habilidad.");
        Console.WriteLine("Presione 'Enter' para realizar su acción.");  

        Console.WriteLine("\nPersonajes:\n");
        Console.WriteLine("El Caballero, su habilidad 'Frénesi' aumenta sus pasos a 10 por un turno.");
        Console.WriteLine("El Mago, su habilidad 'Teletransporte' teletransporta al jugador hacia una moneda.");
        Console.WriteLine("El Ninja, su habilidad 'Arte Secreta' salta un obstáculo.");
        Console.WriteLine("El Minotauro, su habilidad 'Demolición' rompe un obstáculo.");
        Console.WriteLine("El Ladrón, su habilidad 'Desactivar' desactiva las trampas en un radio 5x5.");

        Console.Write("\n\n\nPresione 'Enter' para salir.");

        ConsoleKeyInfo keyInf = Console.ReadKey(true); 
        if(keyInf.Key == ConsoleKey.Enter)
        {
            Console.WriteLine("\n\nSaliendo...");
        }
        else
        {
            Inicio();
        }
    }
    public void Numero_de_Jugadores()
    {
        Console.Clear();
        Console.WriteLine("Introduce el número de jugadores:");
        Console.Write("Tener en cuenta:\nMínimo de jugadores 2.\nMáximo de jugadores 4.\n\n");
        Console.Write("Número de jugadores: ");
        string input = Console.ReadLine();
        if(int.TryParse(input, out control) && control > 1 && control <= 4)
        {
            Seleccion_de_Jugador();
        }
        else
        {
            Numero_de_Jugadores();
        }
    }
    public class Estado
    {
        public string Personaje{get; set;}
        public  int Velocidad{get; set;}
        public string Habilidad{get; set;}
        public int Enfriamiento{get; set;}
    }
    private void Seleccion_de_Jugador()
    {
        Console.Clear();
        List<Estado> opciones = new List<Estado>()
        {
            new Estado {Personaje = "Caballero", Velocidad = 1, Habilidad = "Frénesis", Enfriamiento = 2},
            new Estado {Personaje = "Mago", Velocidad = 2, Habilidad = "Teletransporte", Enfriamiento = 3},
            new Estado {Personaje = "Ninja", Velocidad = 3, Habilidad = "Arte Secreta", Enfriamiento = 2},
            new Estado {Personaje = "Minotauro", Velocidad = 4, Habilidad = "Demolición", Enfriamiento = 3},
            new Estado {Personaje = "Ladrón", Velocidad = 5, Habilidad = "Desactivar", Enfriamiento = 2}
        };

        this.selection_personaje= new List<string>();
        this.selection_habilidad= new List<string>();
        this.selection_velocidad= new List<int>();
        this.selection_enfriamiento= new List<int>();
        for(int i = 0; i < control; i++)
        {
            Console.WriteLine("Selecione su Personaje, escribiendo el número que le corresponde.");
            foreach (var opcion in opciones)
            {
                Console.WriteLine($"{opcion.Velocidad}-{opcion.Personaje}  Velocidad:{opcion.Velocidad}  Habilidad:{opcion.Habilidad}");
            }
            string input = Console.ReadLine();
            int select;
            if(int.TryParse(input, out select) && opciones.Any(x => x.Velocidad == select))
            {
                string save_personaje = opciones.First(x => x.Velocidad == select).Personaje;
                string save_habilidad = opciones.First(x => x.Velocidad == select).Habilidad;
                int save_velocidad = opciones.First(x => x.Velocidad == select).Velocidad;
                int save_enfriamientpo= opciones.First(x => x.Velocidad == select).Enfriamiento;
                selection_personaje.Add(save_personaje);
                selection_habilidad.Add(save_habilidad);
                selection_velocidad.Add(save_velocidad);
                selection_enfriamiento.Add(save_enfriamientpo);
                opciones.Remove(opciones.First(x => x.Velocidad == select));
                Console.Clear();
            }
            else
            {
                i--;
                Console.Clear();
                continue;
            }
        }
        if(control >= 2)
        {
            P1_Personaje = selection_personaje[0];
            P1_Habilidad = selection_habilidad[0];
            P1_Velocidad = selection_velocidad[0];
            P1_Enfriamiento = selection_enfriamiento[0];
            P1_Guardar_Enfriamiento = selection_enfriamiento[0];
            coordenadas[P1_Posicion_x, P1_Posicion_y] = 11;

            P2_Personaje = selection_personaje[1];
            P2_Habilidad = selection_habilidad[1];
            P2_Velocidad = selection_velocidad[1];
            P2_Enfriamiento = selection_enfriamiento[1];
            P2_Guardar_Enfriamiento = selection_enfriamiento[1];
            coordenadas[P2_Posicion_x, P2_Posicion_y] = 12;

            if(control >= 3)
            {
                P3_Personaje = selection_personaje[2];
                P3_Habilidad = selection_habilidad[2];
                P3_Velocidad = selection_velocidad[2];
                P3_Enfriamiento = selection_enfriamiento[2];
                P3_Guardar_Enfriamiento = selection_enfriamiento[2];
                coordenadas[P3_Posicion_x, P3_Posicion_y] = 13;

                if(control >= 4)
                {
                    P4_Personaje = selection_personaje[3];
                    P4_Habilidad = selection_habilidad[3];
                    P4_Velocidad = selection_velocidad[3];
                    P4_Enfriamiento = selection_enfriamiento[3];
                    P4_Guardar_Enfriamiento = selection_enfriamiento[3];
                    coordenadas[P4_Posicion_x, P4_Posicion_y] = 14;
                }
            }
        }
    }
    public void Personaje(int valor, string nombre, string habilidad, int velocidad, int enfriamiento, int x, int y, int puntos )
    {
        puntuacion = puntos;
        guardar_enfriamiento = enfriamiento;
        pasos = velocidad;
        posicion_x = x;
        posicion_y = y;
        ant_posicion_x = x;
        ant_posicion_y = y;

        for(rest = 0; rest < velocidad; rest++)
        { 
            Puntuacion(valor);
            Console.WriteLine($"Turno número : {turno}.");    
            Console.WriteLine($"Turno del {nombre}."); 
            Console.WriteLine($"Habilidad del {nombre} : {habilidad}.");
            Console.WriteLine($"Tiempo de enfriamiento de la habilidad : {guardar_enfriamiento}.");
            Console.WriteLine($"Cantidad de pasos restantes : {pasos}.");
            Console.WriteLine($"Puntuación: {puntuacion}.");

            char lectura;
            string input = Console.ReadLine();

            if(char.TryParse(input, out lectura))
            {
                switch(lectura)
                {
                    case 'w':
                        posicion_x = ant_posicion_x - 1;
                        posicion_y = ant_posicion_y;
                        Movimiento( velocidad, valor);
                        Puntuacion(valor);

                        break;
                    case 's':
                        posicion_x = ant_posicion_x + 1;
                        posicion_y = ant_posicion_y;
                        Movimiento( velocidad, valor);
                        Puntuacion(valor);

                        break;
                    case 'a':
                        posicion_x = ant_posicion_x;
                        posicion_y = ant_posicion_y - 1;
                        Movimiento( velocidad, valor);
                        Puntuacion(valor);

                        break;
                    case 'd':
                        posicion_x = ant_posicion_x;
                        posicion_y = ant_posicion_y + 1;
                        Movimiento( velocidad, valor);
                        Puntuacion(valor);

                        break;   
                    case 'e':
                        if(guardar_enfriamiento == 0)
                        {
                            Console.Clear();
                            Mostrar_Mapa();
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine($"{habilidad} ha sido activada.");
                            Console.ResetColor();
                            if(velocidad == 1)
                            {
                                Habilidad_del_Caballero();
                                Asignacion_de_Enfriamiento(valor);
                            }
                            if(velocidad == 2)
                            {
                                if(cantmon != 0)
                                {
                                    Habilidad_del_Mago(valor);
                                    Asignacion_de_Enfriamiento(valor);
                                }
                                else
                                {
                                    Console.Clear();
                                    Mostrar_Mapa();
                                    Console.ForegroundColor = ConsoleColor.DarkRed;
                                    Console.WriteLine("No hay monedas a las que teletransportarse.");
                                    Console.ResetColor();
                                }
                            }
                            if(velocidad == 3)
                            {
                                if(Comprobar_Casilla_Libre())
                                {
                                    Habilidad_del_Ninja(valor, velocidad);
                                    Asignacion_de_Enfriamiento(valor);
                                }
                                else
                                {   Console.Clear();
                                    Mostrar_Mapa();
                                    Console.ForegroundColor = ConsoleColor.DarkRed;
                                    Console.WriteLine($"No se puede usar la habilidad {habilidad}, no hay obstáculo que saltar o sobrepasa el rango del tablero.");
                                    Console.ResetColor();
                                }
                            }   
                            if(velocidad == 4)
                            {
                                if(Comprobar_Obstáculo())
                                {
                                    Habilidad_del_Minotauro(valor, velocidad);
                                    Asignacion_de_Enfriamiento(valor);
                                }
                                else 
                                {
                                    Console.Clear();
                                    Mostrar_Mapa();
                                    Console.ForegroundColor = ConsoleColor.DarkRed;
                                    Console.WriteLine($"No se puede usar la habilidad {habilidad}, no hay obstáculo que romper o sobrepasa el rango del tablero.");
                                    Console.ResetColor();
                                }  
                            }
                            if(velocidad == 5)
                            {
                                Habilidad_del_Ladrón();
                                Asignacion_de_Enfriamiento(valor);
                            }
                        }
                        else
                        {
                            Console.Clear();
                            Mostrar_Mapa();
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"{habilidad} no puede ser activada, enfriamiento tiene que llegar a cero.");
                            Console.ResetColor();
                        }
                        rest--;
                        break;
                    default:
                        Console.Clear();
                        Mostrar_Mapa();
                        rest--;
                        break;    
                }
            }
            else
            {
                Console.Clear();
                Mostrar_Mapa();
                rest--;
            }
        }
        x = posicion_x;
        y = posicion_y;
        Asignacion_de_Posicion(x, y);
    }
    private void Movimiento(int velocidad, int valor)
    {
        if(posicion_x >= 0 && posicion_y >= 0 && posicion_x < n && posicion_y < n && coordenadas[posicion_x,posicion_y] != 1)
        {
            if(coordenadas[posicion_x, posicion_y] == 11 || coordenadas[posicion_x, posicion_y] == 12 || coordenadas[posicion_x, posicion_y] == 13 || coordenadas[posicion_x, posicion_y] == 14)
            {
                coordenadas[ant_posicion_x, ant_posicion_y] = coordenadas[posicion_x, posicion_y];
                if(coordenadas[ant_posicion_x, ant_posicion_y] == 11)
                {
                    P1_Posicion_x = ant_posicion_x;
                    P1_Posicion_y = ant_posicion_y;
                }
                if(coordenadas[ant_posicion_x, ant_posicion_y] == 12)
                {
                    P2_Posicion_x = ant_posicion_x;
                    P2_Posicion_y = ant_posicion_y;
                }
                if(coordenadas[ant_posicion_x, ant_posicion_y] == 13)
                {
                    P3_Posicion_x = ant_posicion_x;
                    P3_Posicion_y = ant_posicion_y;
                }
                if(coordenadas[ant_posicion_x, ant_posicion_y] == 14)
                {
                    P4_Posicion_x = ant_posicion_x;
                    P4_Posicion_y = ant_posicion_y;
                }
                coordenadas[posicion_x, posicion_y] = valor;
                ant_posicion_x = posicion_x;
                ant_posicion_y = posicion_y;
                Console.Clear();
                Mostrar_Mapa();
                pasos--;
            }
            if(coordenadas[posicion_x, posicion_y] == 7)
            {
                Posicion(valor);

                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Activaste un restador de monedas ...");
                Console.ResetColor();
                restmon--;
                puntuacion--;
            }
            if(coordenadas[posicion_x, posicion_y] == 6)
            {
                canthab--;
                if(valor == 11)
                {
                    P1_Enfriamiento = 0;
                    guardar_enfriamiento = P1_Enfriamiento;
                }
                if(valor == 12)
                {
                    P2_Enfriamiento = 0;
                    guardar_enfriamiento = P2_Enfriamiento;
                }
                if(valor == 13)
                {
                    P3_Enfriamiento = 0;
                    guardar_enfriamiento = P3_Enfriamiento;
                }
                if(valor == 14)
                {
                    P4_Enfriamiento = 0;
                    guardar_enfriamiento = P4_Enfriamiento;
                }
                Posicion(valor);
            }
            if(coordenadas[posicion_x, posicion_y] == 5)
            {
                cantmon--;
                puntuacion++;
                Posicion(valor);
            }
            if(coordenadas[posicion_x, posicion_y] == 4)
            {
                Posicion(valor);

                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("Activaste un potenciador ...");
                Console.ResetColor();
                rest = rest - 10;
                pasos = pasos + 10;
                cantpot--;
            }
            if(coordenadas[posicion_x, posicion_y] == 3)
            {
                Random count = new Random();
                coordenadas[posicion_x, posicion_y] = 0;
                coordenadas[ant_posicion_x, ant_posicion_y] = 0;
                for(int i = 0; i < 1000; i++)
                {
                    posicion_x = count.Next(n);
                    posicion_y = count.Next(n);

                    if(coordenadas[posicion_x, posicion_y] == 0)
                    {
                        coordenadas[posicion_x, posicion_y] = valor;
                        ant_posicion_x = posicion_x;
                        ant_posicion_y = posicion_y;
                        Console.Clear();
                        Mostrar_Mapa();
                        pasos--;
                        break;
                    }
                }
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Activaste una trampa de teletransporte ...");
                Console.ResetColor();
                canttransp--;
            }
            if(coordenadas[posicion_x, posicion_y] == 2)
            {
                Posicion(valor);

                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Activaste una trampa ...");
                Console.ResetColor();
                rest = velocidad;
                canttramp--;
            }
            if(coordenadas[posicion_x, posicion_y] == 0)
            {
                Posicion(valor);
            }    
        } 
        else
        {
            Console.Clear();
            Mostrar_Mapa();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Movimiento no valido");
            Console.ResetColor();
            rest--;  
        } 
    }
    private void Posicion(int valor)
    {
        coordenadas[ant_posicion_x, ant_posicion_y] = 0;
        coordenadas[posicion_x, posicion_y] = valor;
        ant_posicion_x = posicion_x;
        ant_posicion_y = posicion_y;
        Console.Clear();
        Mostrar_Mapa();
        pasos--;   
    }
    private void Puntuacion(int valor)
    {
        if(valor == 11)
        {
            P1_Puntuacion = puntuacion;
        }
        if(valor == 12)
        {
            P2_Puntuacion = puntuacion;
        }
        if(valor == 13)
        {
            P3_Puntuacion = puntuacion;
        }
        if(valor == 14)
        {
            P4_Puntuacion = puntuacion;
        } 
    }
    private void Asignacion_de_Posicion(int x, int y)
    {
        if(control >= 2)
        {
            ++contador;
            if (contador == 1)
            {
                P1_Posicion_x = x;
                P1_Posicion_y = y;
            }
            if (contador == 2)
            {
                P2_Posicion_x = x;
                P2_Posicion_y = y;
                if (control == 2)
                {
                    contador = 0;
                }
            }
           
            if(control >= 3)
            {
                if (contador == 3)
                {
                    P3_Posicion_x = x;
                    P3_Posicion_y = y;
                    if (control == 3)
                    {
                        contador = 0;
                    }
                }

                if(control >= 4)
                {
                    if (contador == 4)
                    {
                        P4_Posicion_x = x;
                        P4_Posicion_y = y;
                        if (control == 4)
                        {
                            contador = 0;
                        }
                    }
                }
            }

        }
    }
    private void Asignacion_de_Enfriamiento(int valor)
    {
        if(valor == 11)
        {
            P1_Enfriamiento = P1_Guardar_Enfriamiento;
            guardar_enfriamiento = P1_Guardar_Enfriamiento;
        }
        if(valor == 12)
        {
            P2_Enfriamiento = P2_Guardar_Enfriamiento;
            guardar_enfriamiento = P2_Guardar_Enfriamiento;
        }
        if(valor == 13)
        {
            P3_Enfriamiento = P3_Guardar_Enfriamiento;
            guardar_enfriamiento = P3_Guardar_Enfriamiento;
        }
        if(valor == 14)
        {
            P4_Enfriamiento = P4_Guardar_Enfriamiento;
            guardar_enfriamiento = P4_Guardar_Enfriamiento;
        }
    }
    private bool Comprobar_Casilla_Libre()
    {
        if(Dentro_del_Rango(2, 0) && coordenadas[posicion_x + 1, posicion_y] == 1 && coordenadas[posicion_x + 2, posicion_y] != 1)
        {
            return true;
        }
        if(Dentro_del_Rango(-2, 0) && coordenadas[posicion_x - 1, posicion_y] == 1 && coordenadas[posicion_x - 2, posicion_y] != 1)
        {
            return true;
        } 
        if(Dentro_del_Rango(0, 2) && coordenadas[posicion_x, posicion_y + 1] == 1 && coordenadas[posicion_x, posicion_y + 2] != 1)
        {
            return true;
        } 
        if(Dentro_del_Rango(0, -2) && coordenadas[posicion_x, posicion_y - 1] == 1 && coordenadas[posicion_x, posicion_y - 2] != 1)
        {
            return true;
        }  
        return false;
    }
    private bool Comprobar_Obstáculo()
    {
        if(Dentro_del_Rango(1, 0) && coordenadas[posicion_x + 1, posicion_y] == 1)
        {
            return true;
        }
        if(Dentro_del_Rango(-1, 0) && coordenadas[posicion_x - 1, posicion_y] == 1)
        {
            return true;
        } 
        if(Dentro_del_Rango(0, 1) && coordenadas[posicion_x, posicion_y + 1] == 1)
        {
            return true;
        } 
        if(Dentro_del_Rango(0, -1) && coordenadas[posicion_x, posicion_y - 1] == 1)
        {
            return true;
        }  
        return false;
    }
    private bool Dentro_del_Rango(int i, int j)
    {
        if(posicion_x + i >= 1 && posicion_y + j >= 1 && posicion_x + i < n - 1 && posicion_y + j < n - 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private void Habilidad_del_Caballero()
    {
        rest = rest - 10;
        pasos = pasos + 10;
    }
    private void Habilidad_del_Mago(int valor)
    {
        Random count = new Random();
        coordenadas[ant_posicion_x,ant_posicion_y] = 0;
        for(int i = 0; i < 1000; i++)
        {
            posicion_x = count.Next(n);
            posicion_y = count.Next(n);

            if(coordenadas[posicion_x, posicion_y] == 0 && Adjunto(posicion_x, posicion_y))
            {
                coordenadas[posicion_x, posicion_y] = valor;
                ant_posicion_x = posicion_x;
                ant_posicion_y = posicion_y;
                Console.Clear();
                Mostrar_Mapa();
                break;
            }
        }
    }
    private void Habilidad_del_Ninja(int valor, int velocidad)
    {
        int i = 0;
        while( i < valor)
        {
            Console.Clear();
            Mostrar_Mapa();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("Presione W S A D para seleccionar la dirección a la que deseas saltar");
            Console.ResetColor();
            char lectura;
            string input = Console.ReadLine();
            if(char.TryParse(input, out lectura))
            {
                switch(lectura)
                {
                    case 'w':
                        if (Dentro_del_Rango(-2, 0) && coordenadas[posicion_x - 1, posicion_y] == 1 && coordenadas[posicion_x - 2, posicion_y] != 1)
                        {
                            posicion_x = ant_posicion_x - 2;
                            posicion_y = ant_posicion_y;
                            Movimiento( velocidad, valor);
                            pasos++;
                            i = valor;
                        }
                        break;
                    case 's':
                        if (Dentro_del_Rango(2, 0) && coordenadas[posicion_x + 1, posicion_y] == 1 && coordenadas[posicion_x + 2, posicion_y] != 1)
                        {
                            posicion_x = ant_posicion_x + 2;
                            posicion_y = ant_posicion_y;
                            Movimiento( velocidad, valor);                            
                            pasos++;
                            i = valor;
                        }
                        break;
                    case 'a':
                        if (Dentro_del_Rango(0, -2) && coordenadas[posicion_x, posicion_y - 1] == 1 && coordenadas[posicion_x, posicion_y - 2] != 1)
                        {
                            posicion_x = ant_posicion_x;
                            posicion_y = ant_posicion_y - 2;
                            Movimiento( velocidad, valor);
                            pasos++;
                            i = valor;
                        }
                        break;
                    case 'd':
                        if (Dentro_del_Rango(0, 2) && coordenadas[posicion_x, posicion_y + 1] == 1 && coordenadas[posicion_x, posicion_y + 2] != 1)
                        {
                            posicion_x = ant_posicion_x;
                            posicion_y = ant_posicion_y + 2;
                            Movimiento( velocidad, valor);
                            pasos++;
                            i = valor;
                        }
                        break;
                }
            }
        }
    }
    private void Habilidad_del_Minotauro(int valor, int velocidad)
    {
        int i = 0;
        while( i < valor)
        {
            Console.Clear();
            Mostrar_Mapa();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("Presione W S A D para seleccionar la dirección en la que deseas romper un obstáculo.");
            Console.ResetColor();
            char lectura;
            string input = Console.ReadLine();
            if(char.TryParse(input, out lectura))
            {
                switch(lectura)
                {
                    case 'w':
                        if (Dentro_del_Rango(-1, 0) && coordenadas[posicion_x - 1, posicion_y] == 1)
                        {
                            posicion_x = ant_posicion_x - 1;
                            posicion_y = ant_posicion_y;
                            coordenadas[posicion_x, posicion_y] = 0;
                            i = valor;
                        }
                        break;
                    case 's':
                        if (Dentro_del_Rango(1, 0) && coordenadas[posicion_x + 1, posicion_y] == 1)
                        {
                            posicion_x = ant_posicion_x + 1;
                            posicion_y = ant_posicion_y;
                            coordenadas[posicion_x, posicion_y] = 0;
                            i = valor;
                        }
                        break;
                    case 'a':
                        if (Dentro_del_Rango(0, -1) && coordenadas[posicion_x, posicion_y - 1] == 1)
                        {
                            posicion_x = ant_posicion_x;
                            posicion_y = ant_posicion_y - 1;
                            coordenadas[posicion_x, posicion_y] = 0;
                            i = valor;
                        }
                        break;
                    case 'd':
                        if (Dentro_del_Rango(0, 1) && coordenadas[posicion_x, posicion_y + 1] == 1)
                        {
                            posicion_x = ant_posicion_x;
                            posicion_y = ant_posicion_y + 1;
                            coordenadas[posicion_x, posicion_y] = 0;
                            i = valor;
                        }
                        break;
                }
            }
        }
        Console.Clear();
        Mostrar_Mapa();
    }
    private void Habilidad_del_Ladrón()
    {
        for(int i = 0; i < direcciones_x2.GetLength(0); i++)    
        {
            int x = posicion_x + direcciones_x2[i, 0];
            int y = posicion_y + direcciones_x2[i, 1];
            if(x >= 0 && y >= 0 && x < n && y < n)
            {
                if(coordenadas[x, y] == 2)
                {
                    coordenadas[x, y] = 0;
                    canttramp--; 
                }
                if(coordenadas[x, y] == 3)
                {
                    coordenadas[x, y] = 0;
                    canttransp--;
                }
                if(coordenadas[x, y] == 7)
                {
                    coordenadas[x, y] = 0;
                    restmon--;
                }
            }
        }
        Console.Clear();
        Mostrar_Mapa();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("Se han eliminado las trampas en un radio de 5x5");
        Console.ResetColor();
    }

    public void Mover_Personaje()
    {
        if(control >= 2)
        {   
            Personaje(11, P1_Personaje, P1_Habilidad, P1_Velocidad, P1_Enfriamiento, P1_Posicion_x, P1_Posicion_y, P1_Puntuacion);
            if (P1_Enfriamiento > 0)
            {
                P1_Enfriamiento--;
            }

            Personaje(12, P2_Personaje, P2_Habilidad, P2_Velocidad, P2_Enfriamiento, P2_Posicion_x, P2_Posicion_y, P2_Puntuacion);
            if (P2_Enfriamiento > 0)
            {
                P2_Enfriamiento--;
            }
            if(control >= 3)
            {
                Personaje(13, P3_Personaje, P3_Habilidad, P3_Velocidad, P3_Enfriamiento, P3_Posicion_x, P3_Posicion_y, P3_Puntuacion);
                if (P3_Enfriamiento > 0)
                {
                    P3_Enfriamiento--;
                }

                if(control >= 4)
                {
                    Personaje(14, P4_Personaje, P4_Habilidad, P4_Velocidad, P4_Enfriamiento, P4_Posicion_x, P4_Posicion_y, P4_Puntuacion);
                    if (P4_Enfriamiento > 0)
                    {
                        P4_Enfriamiento--;
                    }
                }
            }
        }
        turno++;
        if(cantmon != 0)
        {
            Mover_Personaje();
        }
        if(cantmon == 0)
        {
            Victoria();
        }
    }
    private void Victoria()
    {
        Console.Clear();
        Console.WriteLine("\n\n\n\n\n\n\n\n\n\n");
        if(control >= 2)
        {
            if(P1_Puntuacion == Maximo())
            {
                Console.WriteLine($"          Ganador del Juego es el {P1_Personaje}\n");
            }
            if(P2_Puntuacion == Maximo())
            {
                Console.WriteLine($"          Ganador del Juego es el {P2_Personaje}\n");
            }
            if(control >= 3)
            {
                if(P3_Puntuacion == Maximo())
                {
                    Console.WriteLine($"          Ganador del Juego es el {P3_Personaje}\n");
                }
                if(control >= 4)
                {
                    if(P4_Puntuacion == Maximo())
                    {
                        Console.WriteLine($"          Ganador del Juego es  el {P4_Personaje}\n");
                    }
                }
            }
        }
        Console.WriteLine("\nGracias por jugar.");
    }
    private int Maximo()
    {
        int max1 = Math.Max(P1_Puntuacion, P2_Puntuacion);
        int max2 = Math.Max(P3_Puntuacion, P4_Puntuacion);
        int valor = Math.Max(max1, max2);
        return valor;
    }
}
class Inicializacion
{
    static void Main()
    {  
        Game Juego = new Game(); 
        Juego.Inicio();
        Juego.Numero_de_Jugadores();
        Juego.Mostrar_Mapa();
        Juego.Mover_Personaje();
    }
}