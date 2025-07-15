// See https://aka.ms/new-console-template for more information
using System.Data.Common;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

Console.WriteLine("Hello, World!");
using var context = new ModelContext();

if (!context.Provincias.Any() && !context.Cantones.Any() && !context.Distritos.Any())
{
    Console.WriteLine("La base de datos está vacía, por lo que debe ser llenada a partir de los datos del archivo CSV.");
    Console.WriteLine("Procesando...");

    var path = Path.Combine(Directory.GetCurrentDirectory(), "data", "CR.csv");

    if (!File.Exists(path))
    {
        Console.WriteLine($"El archivo {path} no existe. Asegúrate de que el archivo CSV esté en la ubicación correcta.");
        return;
    }

    var lineas = File.ReadAllLines(path).Skip(1); // Saltar la primera línea si es un encabezado

    var provinciasDict = new Dictionary<string, Provincia>();
    var cantonesDict = new Dictionary<string, Canton>();

    foreach (var linea in lineas)
    {
        var partes = linea.Split(',');

        if (partes.Length < 3) continue;

        var provinciaNombre = partes[0].Trim();
        var cantonNombre = partes[1].Trim();
        var distritoNombre = partes[2].Trim();

        // Agregar la Provincia al diccionario si no existe
        if (!provinciasDict.ContainsKey(provinciaNombre))
        {
            var provincia = new Provincia { Nombre = provinciaNombre };
            context.Provincias.Add(provincia);
            context.SaveChanges();
            provinciasDict[provinciaNombre] = provincia;
        }

        var provinciaPK = provinciasDict[provinciaNombre].ProvinciaPK;

        // Agregar el Canton al diccionario si no existe
        var cantonKey = $"{provinciaNombre}|{cantonNombre}";
        if (!cantonesDict.ContainsKey(cantonKey))
        {
            var canton = new Canton { Nombre = cantonNombre, ProvinciaFK = provinciaPK };
            context.Cantones.Add(canton);
            context.SaveChanges();
            cantonesDict[cantonKey] = canton;
        }

        var cantonPK = cantonesDict[cantonKey].CantonPK;

        // Agregar el Distrito
        var distrito = new Distrito { Nombre = distritoNombre, CantonFK = cantonPK };
        context.Distritos.Add(distrito);
    }

    context.SaveChanges();

    Console.WriteLine("Listo.");

}
else
{
    Console.WriteLine("La base de datos ya existe y las tablas están inicializadas.");
}


var provincias = context.Provincias.ToList();

Console.WriteLine("Provincias: ");
foreach (var provincia in provincias)
{
    Console.WriteLine($"{provincia.ProvinciaPK}. {provincia.Nombre}");
}
Console.WriteLine("Indique la provincia: ");
var provinciaSeleccionada = Console.ReadLine();


if (int.TryParse(provinciaSeleccionada, out int provinciaId))
{
    var provinciaexiste = context.Provincias.Any(p => p.ProvinciaPK == provinciaId);
    if (!provinciaexiste)
    {
        Console.WriteLine("ID de provincia no válido.");
        return;
    }

    var provinciaNombre = context.Provincias
        .Where(p => p.ProvinciaPK == provinciaId)
        .Select(p => p.Nombre)
        .FirstOrDefault();

    var cantones = context.Cantones
        .Where(c => c.ProvinciaFK == provinciaId)
        .Select(c => new { c.CantonPK, c.Nombre })
        .ToList();

    System.Console.WriteLine("Cantones: ");
    foreach (var canton in cantones)
    {
        Console.WriteLine($"{canton.CantonPK}. {canton.Nombre}");
    }

    Console.WriteLine("Indique el canton: ");
    var cantonSeleccionado = Console.ReadLine();

    if (int.TryParse(cantonSeleccionado, out int cantonId))
    {
        var cantonexiste = context.Cantones.Any(c => c.CantonPK == cantonId && c.ProvinciaFK == provinciaId);
        if (!cantonexiste)
        {
            Console.WriteLine("ID de canton no válido.");
            return;
        }

        var cantonNombre = context.Cantones
            .Where(c => c.CantonPK == cantonId)
            .Select(c => c.Nombre)
            .FirstOrDefault();


        // Crear el archivo CSV

        var fileName = $"{provinciaSeleccionada}-{cantonSeleccionado}.csv";
        var outputPath = Path.Combine(Directory.GetCurrentDirectory(), "data", fileName);

        using (var writer = new StreamWriter(outputPath, false, Encoding.UTF8))
        {
            // Escribir encabezados
            writer.WriteLine("Provincia,Cantón,Distrito");

            //Escribir datos
            var distritos = context.Distritos
                .Where(d => d.CantonFK == cantonId)
                .Select(d => d.Nombre)
                .ToList();

            foreach (var distrito in distritos)
            {
                writer.WriteLine($"{provinciaNombre},{cantonNombre},{distrito}");
            }
        }

        Console.WriteLine($"Generando y guardando el archivo {fileName}... Listo");

    }
    else
    {
        Console.WriteLine("ID de Cantón no válido.");
    }
}
else
{
    Console.WriteLine("ID de provincia no válido.");
}


/*
void CrearCSV(int provinciaSeleccionada, int cantonSeleccionado)
{
    var fileName = $"{provinciaSeleccionada}-{cantonSeleccionado}.csv";
    var outputPath = Path.Combine(Directory.GetCurrentDirectory(), "data", fileName);

    using (var writer = new StreamWriter(outputPath, false, Encoding.UTF8))
    {
        // Escribir encabezados
        writer.WriteLine("Provincia,Cantón,Distrito");

        //Escribir datos
        var distritos = context.Distritos
            .Where(d => d.CantonFK == cantonSeleccionado)
            .Select(d => d.Nombre)
            .ToList();

        foreach (var distrito in distritos)
        {
            writer.WriteLine($"{provinciaSeleccionada},{cantonSeleccionado},{distrito}");
        }
    }



}
*/

/*
foreach (var provincia in provincias)
{
    Console.WriteLine($"Provincia: {provincia.Nombre}");
    var cantones = context.Cantones.Where(c => c.ProvinciaFK == provincia.ProvinciaPK).ToList();
    foreach (var canton in cantones)
    {
        Console.WriteLine($"  Canton: {canton.Nombre}");
        var distritos = context.Distritos.Where(d => d.CantonFK == canton.CantonPK).ToList();
        foreach (var distrito in distritos)
        {
            Console.WriteLine($"    Distrito: {distrito.Nombre}");
        }
    }
}

/*


string[] lineas = File.ReadAllLines("./data/CR.csv"); // Saltar la primera línea si es un encabezado




foreach (var linea in lineas)
{
    var valores = linea.Split(',');
    Console.WriteLine($"Provincia: {valores[0].Trim()}, Canton: {valores[1].Trim()}, Distrito: {valores[2].Trim()}");
}



foreach (var linea in lineas)
{
    var partes = linea.Split(',');
    if (partes.Length < 3) continue;

    var provinciaNombre = partes[0].Trim();
    var cantonNombre = partes[1].Trim();
    var distritoNombre = partes[2].Trim();

    using (var context = new ModelContext())
    {
        var provincia = context.Provincias
            .FirstOrDefault(p => p.Nombre == provinciaNombre);
        if (provincia == null)
        {
            provincia = new Provincia { Nombre = provinciaNombre };
            context.Provincias.Add(provincia);
            context.SaveChanges();
        }

        var canton = context.Cantones
            .FirstOrDefault(c =>  c.Nombre == cantonNombre && c.ProvinciaFK == provincia.ProvinciaPK);
        if (canton == null)
        {
            canton = new Canton { Nombre = cantonNombre, ProvinciaFK = provincia.ProvinciaPK };
            context.Cantones.Add(canton);
            context.SaveChanges();
        }

        var distrito = new Distrito { Nombre = distritoNombre, CantonFK = canton.CantonPK };
        context.Distritos.Add(distrito);
        context.SaveChanges();
    }
}
*/