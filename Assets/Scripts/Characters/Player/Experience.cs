using System;
using UnityEngine;

public delegate void LevelUpEvent();

[Serializable]
public class Experience
{
    public event LevelUpEvent OnLevelUp = delegate { };
    public event Action OnMaxLevelReached = delegate { };

    /// <summary>
    /// Nivel máximo permitido (0 = sin límite)
    /// </summary>
    public int maxLevel { get; set; }

    /// <summary>
    /// Experiencia actual del jugador
    /// </summary>
    public float exp
    {
        get => _exp;
        private set
        {
            float oldXP = _exp;
            _exp = Mathf.Max(0, value);
        }
    }

    /// <summary>
    /// Nivel actual del jugador
    /// </summary>
    public int Level { get; private set; }

    private const int EXP_NEEDED_PER_LEVEL = 1000;
    private float _exp;

    public Experience(int startLevel = 1, int maxLevel = 0)
    {
        Level = startLevel;
        this.maxLevel = maxLevel;
        exp = 0;
    }

    /// <summary>
    /// Agrega experiencia y maneja subida de nivel.
    /// </summary>
    /// <param name="amount">Cantidad de experiencia a agregar</param>
    public void AddXP(float amount)
    {
        if (amount < 0)
            throw new ArgumentException("XP amount cannot be negative!");

        float oldXP = exp;
        exp += amount;

        while (exp >= GetXPRequiredForLevel(Level))
        {
            if (maxLevel > 0 && Level >= maxLevel)
            {
                exp = 0;
                OnMaxLevelReached.Invoke();
                return;
            }

            exp -= GetXPRequiredForLevel(Level);
            Level++;
            OnLevelUp.Invoke();
        }
    }

    /// <summary>
    /// Devuelve la experiencia necesaria para subir al siguiente nivel.
    /// </summary>
    private float GetXPRequiredForLevel(int level)
    {
        return EXP_NEEDED_PER_LEVEL * level; // Puedes cambiar esto para usar una fórmula más avanzada
    }

    /// <summary>
    /// Reinicia la experiencia a 0 y mantiene el nivel actual.
    /// </summary>
    public void ResetXP()
    {
        exp = 0;
    }

    /// <summary>
    /// Reinicia toda la progresión a un nivel base.
    /// </summary>
    public void ResetProgression(int newLevel = 1)
    {
        Level = newLevel;
        exp = 0;
    }
}

