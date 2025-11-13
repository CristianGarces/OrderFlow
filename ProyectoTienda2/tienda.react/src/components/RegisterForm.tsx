import { useNavigate } from 'react-router-dom';
import { useState } from 'react';
import './Auth.css';

const RegisterForm = () => {
    const navigate = useNavigate();
    const [formData, setFormData] = useState({
        nombre: '',
        email: '',
        password: '',
        confirmPassword: ''
    });

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();

        if (formData.password !== formData.confirmPassword) {
            alert('Las contrasenas no coinciden');
            return;
        }

        try {
            const response = await fetch('https://localhost:7109/api/Auth/register', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(formData),
            });

            if (response.ok) {
                alert('Usuario registrado correctamente');
                navigate('/');
            } else {
                const errorData = await response.json();
                alert(`Error: ${JSON.stringify(errorData)}`);
            }
        } catch (error) {
            console.error('Error:', error);
            alert('Error al conectar con el servidor');
        }
    };

    return (
        <div>
            <h2 className="auth-title">Registro</h2>
            <form onSubmit={handleSubmit} className="auth-form">
                <input
                    type="text"
                    placeholder="Nombre"
                    value={formData.nombre}
                    onChange={(e) => setFormData({ ...formData, nombre: e.target.value })}
                    required
                    className="auth-input"
                />
                <input
                    type="email"
                    placeholder="Email"
                    value={formData.email}
                    onChange={(e) => setFormData({ ...formData, email: e.target.value })}
                    required
                    className="auth-input"
                />
                <input
                    type="password"
                    placeholder="Contrasena"
                    value={formData.password}
                    onChange={(e) => setFormData({ ...formData, password: e.target.value })}
                    required
                    className="auth-input"
                />
                <input
                    type="password"
                    placeholder="Confirmar Contrasena"
                    value={formData.confirmPassword}
                    onChange={(e) => setFormData({ ...formData, confirmPassword: e.target.value })}
                    required
                    className="auth-input"
                />
                <button type="submit" className="auth-button">Registrarse</button>
            </form>
            <div className="auth-link">
                <p>
                    Ya tienes cuenta: 
                    <button onClick={() => navigate('/')}>Iniciar Sesion</button>
                </p>
            </div>
        </div>
    );
};

export default RegisterForm;