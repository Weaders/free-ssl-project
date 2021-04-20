import React, { useState } from 'react';
import { Button, Chip, TextField, FormGroup, FormControlLabel, Checkbox, Grid } from '@material-ui/core'
import { useSelector, useDispatch } from 'react-redux'
import { makeStyles } from '@material-ui/core/styles';
import { getDomains, removeDomain, addDomain } from './../../app/store-data/ssl-form'
import { getTranslate } from './../../app/store-data/main'
import { Card, CardContent, Divider } from '@material-ui/core'

const useStyles = makeStyles((theme) => ({
    rootTags: {
      // border: '1px solid rgba(196, 196, 196, 1)',
      // padding: '15px',
      // 'margin-bottom': '10px',
      // 'border-radius': '16px',
      display: 'flex',
      justifyContent: 'center',
      // flexWrap: 'wrap',
      // marginTop: '10px',
      '& > *': {
        margin: theme.spacing(0.5),
      },
    },
    textStyle: {
      borderTop: '1px solid',
      borderColor: '#d1d1d1',
      paddingTop: '10px',
      fontSize: '20px',
      textAlign: 'center',
      fontStyle: 'italic'
    },
    chipStyle: {
      'font-size': '18px',
      padding: '8px'
    },
    rootInput: {
      flexGrow: 1
    },
    input: {
      width: '100%'
    },
    btn: {
      width: '100%',
      height: '100%'
    }
  }))

export default function DomainsInput() {

    const [text, setText] = useState('');
    const [useWWW, setUseWWW] = useState(true);
    let dispatch = useDispatch();
    let styles = useStyles();
    let domains = useSelector(getDomains).map(d => <Chip key={d} className={styles.chipStyle}  label={d} onDelete={() => dispatch(removeDomain(d))} />);
    let translate = useSelector(getTranslate);

    const addDomainFromTextBox = () => {

      if (useWWW && text.indexOf('www.') !== 0){
        dispatch(addDomain(`www.${text}`));  
      }

      dispatch(addDomain(text));
      setText('');

    }

    let domainsContianer = '';


    if (domains.length){
      domainsContianer = (
        <div>
          <p className={styles.textStyle}>{translate('domains_generated_will')}</p>
          <Card>
            <CardContent className={styles.rootTags}>
              {domains}
            </CardContent>
          </Card>
        </div>
      );
    }

    return(<div>
      
      <div className={styles.rootInput}>
        <Grid container spacing={3}>
          <Grid item xs={9}>
            <TextField value={text} className={styles.input} onChange={e => setText(e.target.value)} id="outlined-basic" label={translate("domain")} variant="outlined" />
          </Grid>
          <Grid item xs={3}>
            <Button onClick={addDomainFromTextBox} disabled={text.length == 0} className={styles.btn} variant="contained" color="secondary">{translate("add_domain")}</Button>
          </Grid>
        </Grid>
      </div>
      <FormGroup row>
        <FormControlLabel
          control={<Checkbox checked={useWWW} onChange={e => setUseWWW(e.target.checked)} name="checkedA" />}
          label={translate("authomatic_add_with_www")}
        />
      </FormGroup>
      {domainsContianer}
    </div>
    );

}