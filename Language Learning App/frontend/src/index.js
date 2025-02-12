import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import App from './App';
import { BrowserRouter } from 'react-router-dom';
import {createRoot} from "react-dom/client";

const container = document.getElementById('root');
const root = createRoot(container); // createRoot(container) if you use Typescript
root.render(
    <React.StrictMode>
        <BrowserRouter>
            <App />
        </BrowserRouter>
    </React.StrictMode>
);