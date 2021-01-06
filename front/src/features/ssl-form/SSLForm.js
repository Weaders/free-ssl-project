import React from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { makeStyles } from '@material-ui/core/styles';
import { selectState, startAsync, changeEnteredDomain, getEnteredDomain, getDomainForUse } from './sslFormSlice'
import TextField from '@material-ui/core/TextField';
import Button from '@material-ui/core/Button';

const useStyles = makeStyles((theme) => ({
    form: {
        width: '100%',
        marginTop: theme.spacing(3) 
    },
    btnSubmit: {
        width: '100%',
        marginTop: '10px'
    },
    width100: {
        width: '100%'
    }
}));

export function SSLForm(){

    const state = useSelector(selectState);
    const userDomain = useSelector(getEnteredDomain);
    const useDomain = useSelector(getDomainForUse);
    const classes = useStyles();    
    const dispatch = useDispatch();


    return (
        <form className={classes.form}>
            <TextField value={userDomain} onChange={e => dispatch(changeEnteredDomain(e.target.value))} className={classes.width100}  id="outlined-basic" label="Domain" variant="outlined" />
            <Button onClick={e => dispatch(startAsync())} className={classes.btnSubmit} fullWidth variant="contained" color="primary" disableElevation>
                Start generate SSL
            </Button>
            {state}
            {useDomain}
        </form>
    )

}
