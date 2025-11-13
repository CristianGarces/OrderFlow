import { useNavigate } from 'react-router-dom';
import { useState } from 'react';
import './Auth.css';

const LoginForm = () => {
    const navigate = useNavigate();
    const [formData, setFormData] = useState({
        email: '',
        password: ''
    });

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();

        try {
            const response = await fetch('https://localhost:7109/api/Auth/login', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(formData),
            });

            if (response.ok) {
                alert('Login correcto');
                navigate('/');
            } else if (response.status === 401) {
                alert('Credenciales incorrectas');
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
            <h2 className="auth-title">Iniciar Sesion</h2>
            <form onSubmit={handleSubmit} className="auth-form">
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
                <button type="submit" className="auth-button">Iniciar Sesion</button>
            </form>
            <div className="auth-link">
                <p>
                    No tienes cuenta: 
                    <button onClick={() => navigate('/registro')}>Registrate</button>
                </p>
            </div>
        </div>
    );
};

export default LoginForm;