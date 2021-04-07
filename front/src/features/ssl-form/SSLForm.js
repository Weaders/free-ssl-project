import React from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { makeStyles } from '@material-ui/core/styles';
import { getCertAsync, startAsync, getChallenges, clearChallenges, getCertData, getDomains } from '../../app/store-data/ssl-form'
import Button from '@material-ui/core/Button';
import ChallengerCheckerCard from './../challenge-checker/ChallengeCheckerCard'
import DomainsInput from './../domains-input/DomainsInput'
import { getTranslate } from './../../app/store-data/main'
import { Typography } from '@material-ui/core';
import { Card, CardContent, Divider } from '@material-ui/core'


const useStyles = makeStyles((theme) => ({
    form: {
        width: '100%',
        marginTop: theme.spacing(3) 
    },
    btnSubmit: {
        width: '100%',
        marginTop: '10px'
    },
    btnReturn: {
        marginBottom: '10px'
    },
    createFileInfo: {
        marginTop: '10px',
        marginBottom: '10px',
        textAlign: 'center'
    },
    width100: {
        width: '100%'
    },
    dividerStyle: {
        marginTop: '10px',
        marginBottom: '10px'
    },
    card: {
        'margin-bottom': '20px',
    },
    alert: {
        'margin-bottom': '10px'
    },
    typographyKey: {
        whiteSpace: 'break-spaces'
    },
    thisIsYourCert: {
        fontSize: '19px'
    }
}));

export function SSLForm(){

    const classes = useStyles();    
    const dispatch = useDispatch();
    let challenges = useSelector(getChallenges);
    let domains = useSelector(getDomains);
    let certData = useSelector(getCertData);
    let translate = useSelector(getTranslate)

    let content = null;
    
    if (certData != null){

        content = (
            <React.Fragment>
                <Card className={classes.createFileInfo}>
                    <CardContent className={classes.thisIsYourCert}>
                        {translate("there_you_certificate")}: <b>Date: {certData.expiredDate.toLocaleString()}</b>
                    </CardContent>
                </Card>
                <Card className={classes.createFileInfo}>
                    <CardContent>
                        <Typography gutterBottom variant="h5" component="h2">
                            {translate("pem_key")}
                        </Typography>
                        <Divider className={classes.dividerStyle} />
                        <Typography className={classes.typographyKey}>
                            {certData.pemKey}
                        </Typography>
                    </CardContent>
                </Card>
                <Card className={classes.createFileInfo}>
                    <CardContent>
                        <Typography gutterBottom variant="h5" component="h2">
                            {translate("private_key")}
                        </Typography>
                        <Divider className={classes.dividerStyle} />
                        <Typography className={classes.typographyKey}>
                            {certData.privateKey}
                        </Typography>
                    </CardContent>
                </Card>
            </React.Fragment>  
        );

    } else if (!challenges.length){

        content = (<React.Fragment>
            <DomainsInput />
            <Button disabled={domains.length <= 0} onClick={e => dispatch(startAsync())} className={classes.btnSubmit} fullWidth variant="contained" color="primary" disableElevation>
                {translate("start_generate_ssl")}
            </Button>
        </React.Fragment>);

    } else {

        content = (<React.Fragment>
            <Button variant="contained" color="default" className={classes.btnReturn} fullWidth onClick={_ => dispatch(clearChallenges())}>
                {translate('return_and_write_more_domains')}
            </Button>
            <Card className={classes.createFileInfo}>
                <CardContent>
                    <Typography>
                        {translate("for_validate_need_create_file")}
                    </Typography>
                </CardContent>
            </Card>
            {challenges.map(ch => (<ChallengerCheckerCard className={classes.card} key={ch.key} location={ch.location} fileName={ch.token} fileKey={ch.key} />))}
            <Button variant="contained" color="primary" fullWidth onClick={_ => dispatch(getCertAsync())}>{translate('i_created_files_give_me_ssl')}</Button>
        </React.Fragment>);
    }

    return (
        <div className={classes.form}>
            {content}
        </div>
    )

}
