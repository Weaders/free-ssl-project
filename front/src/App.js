import React from 'react';
import './App.css';
import Container from '@material-ui/core/Container'
import { SSLForm } from './features/ssl-form/SSLForm'
import axiosDefaults from 'axios/lib/defaults'

function App() {

  // axiosDefaults.xsrfHeaderName = "X-CSRFToken";

  return (
    <Container component="main" maxWidth="xs">
        <SSLForm></SSLForm>
    </Container>
  );
}

export default App;