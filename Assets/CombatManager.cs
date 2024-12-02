using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CombatManager : MonoBehaviour
{
    private StatsManager stats;
    private List<AttackManager> ataquesList;
    private List<StatsManager> personajeList;

    private bool esperandoSeleccionDeAtaque = false; // Bandera para esperar la selección
    private StatsManager personajeActual; // Personaje que está eligiendo el ataque
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
            // Esperar la selección del jugador
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                RealizarAtaque(personajeActual, ataquesActuales, 0);
                esperandoSeleccionDeAtaque = false; // Selección completada
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2) && ataquesActuales.nombresDeAtaques.Count > 1)
            {
                RealizarAtaque(personajeActual, ataquesActuales, 1);
                esperandoSeleccionDeAtaque = false; // Selección completada
            }
            // Agregar más teclas si hay más ataques
        }
    }

    // Convertimos este método en una corutina
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

            // Esperar la selección del jugador
            while (esperandoSeleccionDeAtaque) // Pausa el flujo hasta que el jugador seleccione un ataque
            {
                yield return null; // Espera hasta que se presione una tecla
            }

            // Aquí, puedes agregar lógica para pasar al siguiente turno o realizar otras acciones
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
            print($"{i + 1}. {ataques.nombresDeAtaques[i]} (Daño: {ataques.dañosDeAtaques[i]})");
        }

        // Activar la bandera para esperar la selección del jugador
        esperandoSeleccionDeAtaque = true;
    }

    void RealizarAtaque(StatsManager personaje, AttackManager ataques, int ataqueIndex)
    {
        // Realiza el ataque usando el índice seleccionado
        var nombreAtaque = ataques.nombresDeAtaques[ataqueIndex];
        var dano = ataques.dañosDeAtaques[ataqueIndex];
        print($"{personaje.gameObject.name} usa {nombreAtaque}, causando {dano} de daño.");

        // Aquí puedes agregar la lógica para aplicar el daño al enemigo, etc.
    }
}