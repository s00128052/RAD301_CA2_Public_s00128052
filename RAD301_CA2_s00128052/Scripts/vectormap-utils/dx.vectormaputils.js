﻿/*! 
* DevExtreme (Vector Map)
* Version: 14.2.3
* Build date: Dec 3, 2014
*
* Copyright (c) 2012 - 2014 Developer Express Inc. ALL RIGHTS RESERVED
* EULA: https://www.devexpress.com/Support/EULAs/DevExtreme.xml
*/
"use strict";(function(n,t){function b(n){var o=new Date,t,r=[],e=[],f;try{t=k(n)}catch(u){return r.push("Terminated: "+u.message+" / "+u.description),{errors:r}}t.fileCode!==9994&&r.push("File code: "+t.fileCode+" / expected: 9994");t.version!==1e3&&r.push("File version: "+t.version+" / expected: 1000");try{while(n.position()<t.fileLength)if(f=d(n,t.shapeType,r),f)e.push(f);else{r.push("Terminated");break}n.position()!==t.fileLength&&r.push("File length: "+t.fileLength+" / actual: "+n.position())}catch(u){r.push("Terminated: "+u.message+" / "+u.description)}finally{return{bbox:[t.bbox.xmin,t.bbox.ymin,t.bbox.xmax,t.bbox.ymax],type:i[t.shapeType],shapes:e,errors:r,time:new Date-o}}}function k(n){return{fileCode:n.uint32(!0),unused1:n.uint32(!0),unused2:n.uint32(!0),unused3:n.uint32(!0),unused4:n.uint32(!0),unused5:n.uint32(!0),fileLength:n.uint32(!0)<<1,version:n.uint32(),shapeType:n.uint32(),bbox:{xmin:n.float64(),ymin:n.float64(),xmax:n.float64(),ymax:n.float64(),zmin:n.float64(),zmax:n.float64(),mmin:n.float64(),mmax:n.float64()}}}function d(n,t,u){var f={},e,o,s;return(f.number=n.uint32(!0),e=n.uint32(!0)<<1,o=n.position(),f.shapeType=n.uint32(),f.shapeName=i[f.shapeType],!f.shapeName)?(u.push("Shape #"+f.number+" type: "+f.shapeType+" / unknown"),null):(f.shapeType!==t&&u.push("Shape #"+f.number+" type: "+f.shapeName+" / expected: "+i[t]),r[f.shapeType](n,f),s=n.position(),s-o!==e&&u.push("Shape #"+f.number+" length: "+e+" / actual: "+s-o),f)}function g(){}function nt(n,t){t.coordinates=[n.float64(),n.float64()]}function tt(n,t){for(var u=[n.float64(),n.float64(),n.float64(),n.float64()],f=n.uint32(),r=[],i=0;i<f;++i)r.push([n.float64(),n.float64()]);t.bbox=u;t.coordinates=r}function l(n,t){for(var l=[n.float64(),n.float64(),n.float64(),n.float64()],s=n.uint32(),f=n.uint32(),r=[],h=[],u,e,c=[],o,i=0;i<s;++i)r.push(n.uint32());for(i=0;i<f;++i)h.push([n.float64(),n.float64()]);for(i=0;i<s;++i){for(u=r[i],e=r[i+1]||f,o=[],u=r[i],e=r[i+1]||f;u<e;++u)o.push(h[u]);c.push(o)}t.bbox=l;t.coordinates=c}function it(n){var e=new Date,t=[],i,r,u;try{i=rt(n,t);r=ft(i,t);u=et(n,i.numberOfRecords,i.recordLength,r,t)}catch(f){t.push("Terminated: "+f.message+" / "+f.description)}finally{return{records:u,errors:t,time:new Date-e}}}function rt(n,t){var i={versionNumber:n.uint8(),lastUpdate:new Date(1900+n.uint8(),n.uint8()-1,n.uint8()),numberOfRecords:n.uint32(),headerLength:n.uint16(),recordLength:n.uint16()},f,r,u;for(n.skip(20),f=(i.headerLength-n.position()-1)/32,r=[];f-->0;)r.push(ut(n));return i.fields=r,u=n.uint8(),u!==13&&t.push("Header terminator: "+u+" / expected: 13"),i}function f(n,t){return s.apply(null,n.uint8array(t))}function ut(n){var t={name:f(n,11).replace(a,""),type:s(n.uint8()),length:n.skip(4).uint8(),count:n.uint8()};return n.skip(14),t}function ft(n,t){for(var o=[],f=0,s=n.fields.length,r,i,e=0;f<s;++f)i=n.fields[f],r={name:i.name,parser:u[i.type],length:i.length},r.parser||(r.parser=u._default,t.push("Field "+i.name+" type: "+i.type+" / unknown")),e+=i.length,o.push(r);return e+1!==n.recordLength&&t.push("Record length: "+n.recordLength+" / actual: "+(e+1)),o}function et(n,t,i,r,u){for(var o=0,f,a=r.length,s,h,l=[],c,e;o<t;++o){for(c={},s=n.position(),n.skip(1),f=0;f<a;++f)e=r[f],c[e.name]=e.parser(n,e.length);h=n.position();h-s!==i&&u.push("Record #"+(o+1)+" length: "+i+" / actual: "+h-s);l.push(c)}return l}function ot(n,t){var r=0<=t&&t<=8?h(t):8,i=Number("1E"+r),u=n.float64;n.float64=function(){var n=u.apply(this,arguments);return h(n*i)/i}}function v(n,t){var r,i,u,e;n=n||{};t=t||{};i={};u={};n.shp&&(r=new o(n.shp),ot(r,t.precision||4),i=b(r));n.dbf&&(r=new o(n.dbf),u=it(r));e=[].concat(i.errors||[],u.errors||[]);e.length&&(t.errors=e);for(var s=i.shapes||[],h=u.records||[],f=0,v=s.length>=h.length?s.length:h.length,c,a,l=[];f<v;++f)c=s[f]||{},a=h[f],l.push({type:c.shapeName||null,coordinates:c.coordinates||null,attributes:a||null});return l.length?{bbox:i.bbox||null,type:i.type||null,features:l}:null}function e(n,i,r){function o(){if(u!==t&&f!==t){var n=v({shp:u,dbf:f},h);s&&s(n,e.length?e:null)}}n=n||{};var u,f,e=[],s=typeof r=="function"?r:typeof i=="function"?i:null,h=(typeof i=="function"?null:i)||{};n.shp?w(String(n.shp),function(n,t){u=n;t&&e.push(t);o()}):u=null;n.dbf?w(String(n.dbf),function(n,t){f=n;t&&e.push(t);o()}):f=null;o()}function st(n,t,i){if(n=n||{},n.shp||n.dbf)if(typeof n.shp=="string"||typeof n.dbf=="string")e(n,t,i);else return v(n,t);else if(typeof n=="string")y.test(n)?e({shp:n},t,i):p.test(n)?e({dbf:n},t,i):e({shp:n+".shp",dbf:n+".dbf"},t,i);else return null}function o(n){this._dataView=new DataView(n);this._position=0}function w(n,t){var i=new XMLHttpRequest;i.onreadystatechange=function(){if(this.readyState===4){var n=this.statusText==="OK"?null:this.statusText;t(n?null:this.response,n)}};i.open("GET",n);i.responseType="arraybuffer";i.setRequestHeader("X-Requested-With","XMLHttpRequest");i.send(null)}var i={},r,s,a,u,h,y,p,c;i[0]="Null";i[1]="Point";i[3]="Polyline";i[5]="Polygon";i[8]="MultiPoint";r={};r[0]=g;r[1]=nt;r[8]=tt;r[3]=l;r[5]=l;s=String.fromCharCode;a=/\0*$/gi;u={};u._default=function(n,t){return n.skip(t),null};u.C=function(n,t){var i=f(n,t);return i.trim()};u.N=function(n,t){var i=f(n,t);return parseFloat(i,10)};u.D=function(n,t){var i=f(n,t);return new Date(i.substring(0,4),i.substring(4,6)-1,i.substring(6,8))};h=Math.round;y=/\.shp$/i;p=/\.dbf$/i;o.prototype={constructor:o,position:function(){return this._position},skip:function(n){return this._position+=n,this},uint8array:function(n){for(var i=this._dataView,r=0,t=[];r++<n;)t.push(i.getUint8(this._position++));return t},uint8:function(){return this._dataView.getUint8(this._position++)},uint16:function(n){var t=this._dataView.getUint16(this._position,!n);return this._position+=2,t},uint32:function(n){var t=this._dataView.getUint32(this._position,!n);return this._position+=4,t},float64:function(n){var t=this._dataView.getFloat64(this._position,!n);return this._position+=8,t}};c=n.DevExpress=n.DevExpress||{};(c.viz=c.viz||{}).vectormaputils={parse:st}})(this)