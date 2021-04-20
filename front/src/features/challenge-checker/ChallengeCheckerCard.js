import React from 'react'
import { Card, CardContent, Divider } from '@material-ui/core'
import { useSelector } from 'react-redux';
import { getTranslate } from './../../app/store-data/main'
import { makeStyles } from '@material-ui/core/styles';
import CopyIcon from './../copy-icon/CopyIcon'

const useStyles = makeStyles((theme) => ({
    hrefTitle: {
        'font-size': '17px'
    },
    keyTitle: {
        'font-size': '17px',
        'color': '#392a2a'
    },
    keyText: {
        'font-weight': 'bold'
    },
    divider: {
        'margin-top': '10px',
        'margin-bottom': '10px'
    },
    copyContent: {
        marginLeft: '10px'
    }
}));

export default function ChallengeCheckerCard(props) {

    const translate = useSelector(getTranslate);
    const styles = useStyles();

    return(<Card className={props.className}>
        <CardContent>
            <div>
                <p className={styles.hrefTitle}>
                    {translate('filename')}:  
                     <b>{props.fileName}</b>
                     <CopyIcon text={props.fileName} />
                </p>
                <p>{translate("file_link")}: <a href={props.location}>{props.location}</a></p>
                <Divider className={styles.divider} />
                <p className={styles.keyTitle}>{translate('file_must_contain')}:</p>
                <p className={styles.keyText}>
                    {props.fileKey}
                    <CopyIcon text={props.fileKey} />
                </p>
            </div>
        </CardContent>
    </Card>);
}