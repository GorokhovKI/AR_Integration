import numpy as np
import matplotlib.pyplot as plt

# Задаем частоту и вычисляем длину волны
frequency = 2.4e9  # 2.4 ГГц
c = 3e8  # Скорость света в вакууме, м/с
wavelength = c / frequency  # Длина волны, м
dipole_length = wavelength / 2 # Длина диполя

# Определяем угол для расчета диаграммы направленности
theta = np.linspace(0, 2 * np.pi, 360)

# Расчет направленности дипольной антенны в горизонтальной плоскости (Э-плоскость)
# Для идеального диполя: E(θ) = sin(θ)
k = 2 * np.pi / wavelength
denominator = k * dipole_length * np.sin(theta)
denominator[denominator == 0] = np.nan  # Заменяем 0 на NaN, чтобы исключить из графика
F_plane = np.sin(k * dipole_length * np.sin(theta)) / denominator

# Создаем фигуру и оси для графика
fig, ax = plt.subplots(subplot_kw={'projection': 'polar'})

# Диаграмма направленности в Э-плоскости
ax.plot(theta, F_plane)
ax.set_title('Горизонтальная плоскость')

plt.show()

def calculate_reflection_coefficient(Z_L, Z_0):
    return (Z_L - Z_0) / (Z_L + Z_0)

def calculate_wfr(reflection_coefficient):
    if abs(reflection_coefficient) == 0:
        return float('inf')
    return 1 / abs(reflection_coefficient)


def calculate_vswr(reflection_coefficient):
  return (1 + abs(reflection_coefficient)) / (1 - abs(reflection_coefficient))

# Параметры дипольной антенны и линии передачи
Z_L = 73.2  # Импеданс диполя, Ом
Z_0 = 50  # Характеристический импеданс линии передачи, Ом

print(f"Рабочая частота: {frequency} Гц")
print(f"Скорость света: {c} м/c")
print(f"Длина волны: {wavelength} м")
print(f"Длина диополя: {dipole_length}")
# Расчет коэффициента отражения
gamma = calculate_reflection_coefficient(Z_L, Z_0)
print(f"Коэффициент отражения (Γ): {gamma}")

# Расчет коэффициента бегущей волны (КБВ)
wfr = calculate_wfr(gamma)
print(f"Коэффициент бегущей волны (КБВ): {wfr}")


# Расчет коэффициента стоячей волны (КСВ)
vswr = calculate_vswr(gamma)
print(f"Коэффициент стоячей волны (КСВ): {vswr}")

wfr2 = 1/vswr
print(f"Коэффициент бегущей волны (КСВ): {wfr}")
# Оценка формы волнового фронта на некотором расстоянии от антенны (аппроксимация сферической волной)
def estimate_wavefront_shape(distance):
    wavefront_amplitude = dipole_length / distance
    return wavefront_amplitude

def calculate_max_transmission_distance_wfr(gamma, frequency, speed_of_light):
    return (speed_of_light / (2 * frequency)) * (1 / abs(gamma))

# Расчет максимальной дальности передачи на основе wfr
max_distance_wfr = calculate_max_transmission_distance_wfr(gamma, frequency, c)
print(f"Максимальная дальность передачи сигнала (на основе wfr): {max_distance_wfr} м")


