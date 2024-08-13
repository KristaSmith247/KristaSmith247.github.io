import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import './styles.css';

import App from './App';

const root = createRoot(document.getElementById("root"));
root.render(
    // change "root" component to an App component (App.js)
    <StrictMode>
        <App />
    </StrictMode>
);
