# Tarea Programada 4 - Consola CSV

**Nombre:** Tayron Jhossue Guzman Campos  
**Carné:** FI23032601  
**Repositorio:** [https://github.com/TayronGC/TP4-FI23032601.git](https://github.com/TayronGC/TP4-FI23032601.git)

---

## 📦 Comandos `dotnet` utilizados

```bash
dotnet new sln -n TP4
dotnet new console -o Tarea
dotnet sln add Tarea
dotnet --list-sdks
dotnet tool list --global
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
```
## Paginas web que me ayudaron
Durante el desarrollo del proyecto, consulté los siguientes recursos 
- Code First con Entity Framework y SQLite en C# .Net 🥞🥞:  
  [https://www.youtube.com/watch?v=TyRrFt2fUJs](https://www.youtube.com/watch?v=TyRrFt2fUJs)  
  _Para ver la todo lo relacionado con Code First con Entity Framework y SQLite

- Leer Archivos CSV con C# - Bytes:  
  [https://www.youtube.com/watch?v=Z_G1e-CMmvU](https://www.youtube.com/watch?v=Z_G1e-CMmvU)  
  _Para leer el archivo CR.csv y pasar la información a la base de datos

- How to create CSV file programmatically using C#?:  
  [https://www.youtube.com/watch?v=Zk3vmuJgGFU](https://www.youtube.com/watch?v=Zk3vmuJgGFU)  
  _Para crear un archivo csv y escribir

- Writing to a CSV File in C# (Simple):  
  [https://www.youtube.com/watch?v=vDpww7HsdnM](https://www.youtube.com/watch?v=vDpww7HsdnM)  
  _Para escribir en el archivo csv




## 🤖 Prompts (consultas y respuestas) de ChatGPT utilizados

---


### 🟠 1. Pregunta sobre como escribir en archivos csv

**Entrada:**

> como hago para crear y escribir un archivo csv sin la libreria CsvHelper?


**Razón:**  
Vi algunos videos que lo usaban con una libreria CsvHelper, y vi 2 videos sin ella pero lo hacian de forma distinta, y queria ver como ChatGPT me lo podria dar
**Respuesta de ChatGPT:**

Puedes crear y escribir un archivo CSV en C# sin necesidad de usar CsvHelper utilizando únicamente las clases básicas del .NET Framework, como StreamWriter. Aquí tienes un ejemplo completo:

✅ Ejemplo: Crear y escribir un archivo CSV en C# (sin librerías externas)

```csharp
using System;
using System.IO;
using System.Text;

class Program
{
    static void Main()
    {
        string rutaArchivo = "datos.csv";

        // Crear datos de ejemplo
        string[] encabezado = { "Provincia", "Cantón", "Distrito" };
        string[][] datos = new string[][]
        {
            new string[] { "San José", "San José", "Carmen" },
            new string[] { "San José", "San José", "Merced" },
            new string[] { "San José", "San José", "Hospital" },
            new string[] { "San José", "San José", "Catedral" },
            new string[] { "San José", "San José", "Zapote" }
        };

        // Crear y escribir el archivo CSV
        using (StreamWriter writer = new StreamWriter(rutaArchivo, false, Encoding.UTF8))
        {
            // Escribir encabezado
            writer.WriteLine(string.Join(",", encabezado));

            // Escribir filas de datos
            foreach (var fila in datos)
            {
                // Escapar comas o comillas si es necesario
                string linea = string.Join(",", fila);
                writer.WriteLine(linea);
            }
        }

        Console.WriteLine($"Archivo CSV creado en: {Path.GetFullPath(rutaArchivo)}");
    }
}

```

📌 Notas importantes
- Si un valor tiene comas o comillas, deberías escaparlas así:
```csharp
string valorEscapado = "\"" + valor.Replace("\"", "\"\"") + "\"";
```
- Para valores seguros (sin comas, saltos de línea o comillas dobles), puedes usar directamente string.Join.

- Este método funciona perfectamente para archivos simples y pequeños. Para cosas más complejas como codificaciones específicas, tipos avanzados, mapeo de objetos, etc., CsvHelper es más práctico.


---