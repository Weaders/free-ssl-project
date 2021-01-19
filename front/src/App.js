import React from 'react';
import { useSelector } from 'react-redux'
import { getIsLoading } from './app/store-data/main'
import { AppBar, Toolbar, Typography, makeStyles, Backdrop, CircularProgress } from '@material-ui/core'
import Container from '@material-ui/core/Container'
import { SSLForm } from './features/ssl-form/SSLForm'
import { ToastContainer } from 'react-toastify';

import 'react-toastify/dist/ReactToastify.css';


const useStyles = makeStyles({
  header: {
    zIndex: 2
  },
  backdrop: {
    zIndex: 10
  },
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

  const styles = useStyles();
  const isLoad = useSelector(getIsLoading);

  return (
    <React.Fragment>
      <AppBar position="sticky" color="default" className={styles.header}>
        <Toolbar>
          <Typography variant="h6" color="inherit" noWrap className={styles.headaerText}>
            SSL Get
          </Typography>
          <div className={styles.gitIconContainer}>
            <a href="https://github.com/Weaders/free-ssl-project"  rel="noreferrer" target="_blank"><img alt="git link" src="GitHub-Mark-120px-plus.png" className={styles.gitIcon} /></a>
          </div>
        </Toolbar>
      </AppBar>
      <Container component="main">
        <SSLForm />
      </Container>
      <Backdrop open={isLoad} className={styles.backdrop}>
        <CircularProgress color="primary"/>
      </Backdrop>
      <ToastContainer />
    </React.Fragment>
  );
}

export default App;