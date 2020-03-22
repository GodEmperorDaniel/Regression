using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Fungus;

public class TMP_Tag
{
	public static List<TMP_Tag> allTags = new List<TMP_Tag>() {
		new TMP_Tag("align=","/align",1,false),
		new TMP_Tag("alpha=","/alpha",1,false),
		new TMP_Tag("cspace=","/cspace",1,false),
		new TMP_Tag("font=","/font",1,false),
		new TMP_Tag("indent=","/indent",1,false),
		new TMP_Tag("line-height=","/line-height",1,false),
		new TMP_Tag("line-indent=","/line-indent",1,false),
		new TMP_Tag("link=","/link",1,false),
		new TMP_Tag("lowercase","/lowercase",0,false),
		new TMP_Tag("uppercase","/uppercase",0,false),
		new TMP_Tag("smallcaps","/smallcaps",0,false),
		new TMP_Tag("margin=","/margin",1,false),
		new TMP_Tag("margin-left=","/margin",1,false),
		new TMP_Tag("margin-right=","/margin",1,false),
		new TMP_Tag("mark=","/mark",1,true),
		new TMP_Tag("mspace=","/mspace",1,false),
		new TMP_Tag("noparse","/noparse",0,false),
		new TMP_Tag("nobr","/nobr",0,false),
		new TMP_Tag("pos=","",1,false),
		new TMP_Tag("s","/s",0,true),
		new TMP_Tag("u","/u",0,true),
		new TMP_Tag("style=","/style",1,false),
		new TMP_Tag("sup","/sup",0,false),
		new TMP_Tag("sub","/sub",0,false),
		new TMP_Tag("voffset=","/voffset",1,false),
		new TMP_Tag("width=","/width",1,false),
		new TMP_Tag("page","",0,false),
		new TMP_Tag("space=","",1,false),
		new TMP_Tag("sprite=","",3,true),
		new TMP_Tag("sprite index=","",2,true),
		new TMP_Tag("sprite name=","",2,true),
	};

	public readonly int parameterCount = 0;
	public readonly string open;
	public readonly string close;
	public readonly bool shouldHide;

	public TMP_Tag (string open, string close, int paramCount, bool shouldHide) {
		this.open = open;
		this.close = close;
		this.shouldHide = shouldHide;
		parameterCount = paramCount;
	}
}
