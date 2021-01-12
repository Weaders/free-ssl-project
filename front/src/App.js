import React from 'react';
import './App.css';
import Container from '@material-ui/core/Container'
import { SSLForm } from './features/ssl-form/SSLForm'

function App() {
  return (
    <Container component="main">
        <SSLForm/>
    </Container>
  );
}

export default App;