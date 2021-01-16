import React from 'react';
import './App.css';
import { AppBar, Toolbar, Typography, makeStyles } from '@material-ui/core'
import Container from '@material-ui/core/Container'
import { SSLForm } from './features/ssl-form/SSLForm'
import c from './app/config'

const useStyles = makeStyles({
  headaerText: {
    'font-style': 'italic',
    'padding-right': '10px'
  },
  gitIcon: {
    width: '32px'
  },
  gitIconContainer: {
    position: 'absolute',
    right: '10px'
  }
});

function App() {

  console.log(c, process.env);

  const styles = useStyles();

  return (
    <React.Fragment>
      <AppBar position="sticky" color="default" >
        <Toolbar>
          <Typography variant="h6" color="inherit" noWrap className={styles.headaerText}>
            SSL Get
          </Typography>
          <div className={styles.gitIconContainer}>
            <a href="https://github.com/Weaders/free-ssl-project" target="_blank"><img src="GitHub-Mark-120px-plus.png" className={styles.gitIcon} /></a>
          </div>
        </Toolbar>
      </AppBar>
      <Container component="main">
        <SSLForm />
      </Container>
    </React.Fragment>
  );
}

export default App;