// Archivo: wwwroot/js/dashboard.js

// Componentes de Recharts que necesitamos
const { BarChart, Bar, XAxis, YAxis, Tooltip, Legend, ResponsiveContainer } = Recharts;
const { useState, useEffect } = React;

function Dashboard() {
    // Estado para almacenar los datos
    const [ventasDiarias, setVentasDiarias] = useState([]);
    const [ventasMensuales, setVentasMensuales] = useState([]);
    const [ventasRecientes, setVentasRecientes] = useState([]);
    const [estadisticas, setEstadisticas] = useState({
        ventasTotales: "$ 0",
        gastosTotales: "$ 0",
        ventasDelMes: 0,
        tendenciaVentas: 0,
        gastosDelMes: 0,
        tendenciaGastos: 0
    });
    const [verEnTiempoReal, setVerEnTiempoReal] = useState(true);
    const [cargando, setCargando] = useState(true);

    // Cargar datos del servidor
    useEffect(() => {
        fetch('/Dashboard/ObtenerDatosDashboard')
            .then(response => response.json())
            .then(data => {
                // Procesar estadísticas generales
                setEstadisticas({
                    ventasTotales: `$ ${formatearNumero(data.estadisticasGenerales.ventasTotales)}`,
                    gastosTotales: `$ ${formatearNumero(data.estadisticasGenerales.gastosTotales)}`,
                    ventasDelMes: Math.round(data.estadisticasGenerales.ventasDelMes / 1000),
                    tendenciaVentas: data.estadisticasGenerales.tendenciaVentas,
                    gastosDelMes: Math.round(data.estadisticasGenerales.gastosDelMes / 1000),
                    tendenciaGastos: data.estadisticasGenerales.tendenciaGastos
                });

                // Procesar ventas diarias
                setVentasDiarias(data.ventasDiarias);

                // Procesar ventas mensuales
                setVentasMensuales(data.ventasMensuales);

                // Procesar ventas recientes
                setVentasRecientes(data.ventasRecientes);

                setCargando(false);
            })
            .catch(error => {
                console.error('Error al cargar datos del dashboard:', error);
                setCargando(false);
            });
    }, []);

    // Función para formatear números grandes
    function formatearNumero(numero) {
        return new Intl.NumberFormat('es-CO').format(Math.round(numero));
    }

    if (cargando) {
        return (
            <div className="flex justify-center items-center h-64">
                <p className="text-lg">Cargando datos del dashboard...</p>
            </div>
        );
    }

    return (
        <div className="bg-pink-50 min-h-screen p-4">
            <h1 className="text-2xl font-bold mb-6">Dashboard</h1>

            {/* Estadísticas principales */}
            <div className="grid grid-cols-1 md:grid-cols-2 gap-4 mb-6">
                <div className="bg-white p-4 rounded-lg shadow">
                    <h2 className="text-gray-500 text-sm">Ventas totales</h2>
                    <div className="flex justify-between items-center">
                        <p className="text-xl font-bold">{estadisticas.ventasTotales}</p>
                        <div className="flex items-center">
                            <p className="mr-2">{estadisticas.ventasDelMes}K/Mes</p>
                            <span className={`text-xs ${estadisticas.tendenciaVentas >= 0 ? 'text-pink-500' : 'text-red-500'}`}>
                                {estadisticas.tendenciaVentas >= 0 ? '↗' : '↘'} {Math.abs(estadisticas.tendenciaVentas)}%
                            </span>
                        </div>
                    </div>
                    <div className="w-full h-2 bg-gray-200 rounded-full mt-2">
                        <div className="h-full bg-yellow-400 rounded-full" style={{ width: '75%' }}></div>
                    </div>
                </div>

                <div className="bg-white p-4 rounded-lg shadow">
                    <h2 className="text-gray-500 text-sm">Gastos totales</h2>
                    <div className="flex justify-between items-center">
                        <p className="text-xl font-bold">{estadisticas.gastosTotales}</p>
                        <div className="flex items-center">
                            <p className="mr-2">{estadisticas.gastosDelMes}K/Mes</p>
                            <span className={`text-xs ${estadisticas.tendenciaGastos >= 0 ? 'text-yellow-500' : 'text-green-500'}`}>
                                {estadisticas.tendenciaGastos >= 0 ? '↗' : '↘'} {Math.abs(estadisticas.tendenciaGastos)}%
                            </span>
                        </div>
                    </div>
                    <div className="w-full h-2 bg-gray-200 rounded-full mt-2">
                        <div className="h-full bg-yellow-400 rounded-full" style={{ width: '60%' }}></div>
                    </div>
                </div>
            </div>

            {/* Gráficos y datos en tiempo real */}
            <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
                <div className="bg-white p-4 rounded-lg shadow md:col-span-2">
                    <h2 className="text-gray-500 text-sm mb-4">Ventas diarias</h2>
                    <ResponsiveContainer width="100%" height={200}>
                        <BarChart data={ventasDiarias}>
                            <XAxis dataKey="dia" />
                            <YAxis />
                            <Tooltip />
                            <Legend />
                            <Bar dataKey="ventasDiarias" name="Ventas" fill="#EC4899" />
                            <Bar dataKey="comprasDiarias" name="Compras" fill="#D1D5DB" />
                        </BarChart>
                    </ResponsiveContainer>
                </div>

                <div className="bg-white p-4 rounded-lg shadow">
                    <div className="flex justify-between items-center mb-4">
                        <h2 className="text-gray-500 text-sm">Ventas en tiempo real</h2>
                        <div className="flex items-center">
                            <span className="text-xs mr-2">Ver en vivo</span>
                            <button
                                className={`w-10 h-5 rounded-full flex ${verEnTiempoReal ? 'bg-pink-500 justify-end' : 'bg-gray-300 justify-start'}`}
                                onClick={() => setVerEnTiempoReal(!verEnTiempoReal)}
                            >
                                <span className="w-5 h-5 bg-white rounded-full shadow"></span>
                            </button>
                        </div>
                    </div>

                    <div className="space-y-3">
                        {ventasRecientes.map(venta => (
                            <div key={venta.id} className="flex items-center p-2 border-b">
                                <div className="bg-gray-200 w-8 h-8 rounded-full mr-2"></div>
                                <div className="flex-1">
                                    <p className="font-semibold">{venta.producto}</p>
                                    <p className="text-xs text-gray-500">ID: {venta.codigo}</p>
                                </div>
                            </div>
                        ))}
                    </div>
                </div>

                <div className="bg-white p-4 rounded-lg shadow md:col-span-3">
                    <h2 className="text-gray-500 text-sm mb-4">Ventas mensuales</h2>
                    <ResponsiveContainer width="100%" height={200}>
                        <BarChart data={ventasMensuales}>
                            <XAxis dataKey="mes" />
                            <YAxis />
                            <Tooltip />
                            <Legend />
                            <Bar dataKey="ventas" name="Ventas" fill="#ECD444" />
                            <Bar dataKey="compras" name="Compras" fill="#D1D5DB" />
                        </BarChart>
                    </ResponsiveContainer>
                </div>
            </div>
        </div>
    );
}

// Renderizar el componente cuando el DOM esté listo
document.addEventListener('DOMContentLoaded', function () {
    const dashboardContainer = document.getElementById('dashboard-container');
    if (dashboardContainer) {
        ReactDOM.render(React.createElement(Dashboard), dashboardContainer);
    }
});