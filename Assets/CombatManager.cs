using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CombatManager : MonoBehaviour
{
    private StatsManager stats;
    private List<AttackManager> ataquesList;
    private List<StatsManager> personajeList;

    private bool esperandoSeleccionDeAtaque = false; // Bandera para esperar la selecci�n
    private StatsManager personajeActual; // Personaje que est� eligiendo el ataque
    private AttackManager ataquesActuales; // Ataques del personaje actual

    // Start is called before the first frame update
    void Start()
    {
        personajeList = GetComponentsInChildren<StatsManager>().ToList();
        ataquesList = GetComponentsInChildren<AttackManager>().ToList();
        StartCoroutine(Combate()); // Llamamos a la corutina
    }

    // Update is called once per frame
    void Update()
    {
        if (esperandoSeleccionDeAtaque)
        {
            // Esperar la selecci�n del jugador
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                RealizarAtaque(personajeActual, ataquesActuales, 0);
                esperandoSeleccionDeAtaque = false; // Selecci�n completada
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2) && ataquesActuales.nombresDeAtaques.Count > 1)
            {
                RealizarAtaque(personajeActual, ataquesActuales, 1);
                esperandoSeleccionDeAtaque = false; // Selecci�n completada
            }
            // Agregar m�s teclas si hay m�s ataques
        }
    }

    // Convertimos este m�todo en una corutina
    IEnumerator Combate()
    {
        // Ordenar los personajes por velocidad
        personajeList = personajeList.OrderByDescending(p => p.velocidad).ToList();

        // Iterar sobre los personajes
        foreach (var personaje in personajeList)
        {
            var stats = personaje.GetComponent<StatsManager>();
            var ataques = personaje.GetComponent<AttackManager>();

            // Mostrar los ataques disponibles para el personaje
            SeleccionarAtaque(personaje, ataques);

            // Esperar la selecci�n del jugador
            while (esperandoSeleccionDeAtaque) // Pausa el flujo hasta que el jugador seleccione un ataque
            {
                yield return null; // Espera hasta que se presione una tecla
            }

            // Aqu�, puedes agregar l�gica para pasar al siguiente turno o realizar otras acciones
        }
    }

    void SeleccionarAtaque(StatsManager personaje, AttackManager ataques)
    {
        // Asignar el personaje actual y sus ataques
        personajeActual = personaje;
        ataquesActuales = ataques;

        // Mostrar los ataques disponibles para el personaje
        print($"{personaje.gameObject.name}, selecciona un ataque:");

        for (int i = 0; i < ataques.nombresDeAtaques.Count; i++)
        {
            print($"{i + 1}. {ataques.nombresDeAtaques[i]} (Da�o: {ataques.da�osDeAtaques[i]})");
        }

        // Activar la bandera para esperar la selecci�n del jugador
        esperandoSeleccionDeAtaque = true;
    }

    void RealizarAtaque(StatsManager personaje, AttackManager ataques, int ataqueIndex)
    {
        // Realiza el ataque usando el �ndice seleccionado
        var nombreAtaque = ataques.nombresDeAtaques[ataqueIndex];
        var dano = ataques.da�osDeAtaques[ataqueIndex];
        print($"{personaje.gameObject.name} usa {nombreAtaque}, causando {dano} de da�o.");

        // Aqu� puedes agregar la l�gica para aplicar el da�o al enemigo, etc.
    }
}