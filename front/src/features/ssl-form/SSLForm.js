import React from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { makeStyles } from '@material-ui/core/styles';
import { getCertAsync, startAsync, getChallenges } from '../../app/store-data/ssl-form'
import Button from '@material-ui/core/Button';
import ChallengerCheckerCard from './../challenge-checker/ChallengeCheckerCard'
import DomainsInput from './../domains-input/DomainsInput'
import { getTranslate } from './../../app/store-data/main'


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
    },
    card: {
        'margin-bottom': '20px',
    }
}));

export function SSLForm(){

    const classes = useStyles();    
    const dispatch = useDispatch();
    let challenges = useSelector(getChallenges);
    let translate = useSelector(getTranslate)

    let buttonForGenerate = null;
    let buttonForCheck = null;

    if (!challenges.length){
        buttonForGenerate = (
            <Button onClick={e => dispatch(startAsync())} className={classes.btnSubmit} fullWidth variant="contained" color="primary" disableElevation>
                {translate("start_generate_ssl")}
            </Button>
        );
    } else {
        buttonForCheck = (
            <Button onClick={_ => dispatch(getCertAsync())}>Check</Button>
        );
    }

    let htmlChallenges = challenges.map(ch => (<ChallengerCheckerCard className={classes.card} key={ch} location={ch.location} fileName={ch.token} fileKey={ch.key} />))
    return (
        <form className={classes.form}>
            <DomainsInput />
            {buttonForGenerate}
            {htmlChallenges}
            {buttonForCheck}
        </form>
    )

}
