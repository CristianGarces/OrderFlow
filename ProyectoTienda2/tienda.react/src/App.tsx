import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import LoginForm from './components/LoginForm';
import RegisterForm from './components/RegisterForm';
import './App.css';

function App() {
    return (
        <Router>
            <div className="App">
                <h1>Tienda App</h1>
                <Routes>
                    <Route path="/" element={<LoginForm />} />
                    <Route path="/registro" element={<RegisterForm />} />
                </Routes>
            </div>
        </Router>
    );
}

export default App;